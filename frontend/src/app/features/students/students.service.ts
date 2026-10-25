import { inject, Injectable, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ValidationErrors, Validators } from '@angular/forms';
import { switchMap, tap } from 'rxjs';
import { ApiService } from '../../core/services/http-client/api.service';
import { strongPassword } from '../../core/validators/password.validator';
import { StudentRow } from './students.model';

export type ModoPanel = 'editar' | 'ver';

const TONOS = ['violet', 'coral', 'mint', 'sky', 'amber'];

/** Error de grupo: la confirmación debe ser igual a la contraseña. */
function passwordsMatch(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirm = group.get('passwordConfirm')?.value;
  return password === confirm ? null : { mismatch: true };
}

@Injectable({ providedIn: 'root' })
export class StudentsService {

  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);

  public students = signal<StudentRow[]>([]);

  // ---------------------------------------------------------------------------
  // Datos
  // ---------------------------------------------------------------------------

  public getStudents$() {
    return this.api.getAll<StudentRow>('Student/get-student-full').pipe(
      tap(res => this.students.set(res.data ?? []))
    );
  }

  /** Edita los datos del estudiante con la sesión abierta y recarga la lista. */
  public updateStudent$(raw: { firstName: string; lastName: string; email: string; identification: string; password: string }) {
    const body = {
      student: {
        firstName: raw.firstName.trim(),
        lastName: raw.lastName.trim(),
        email: raw.email.trim(),
        identification: Number(raw.identification),
        password: raw.password || null
      }
    };

    return this.api.updateOurParams<string, typeof body>('Student/update-student', body).pipe(
      switchMap(() => this.getStudents$())
    );
  }

  /** Elimina el registro del estudiante con la sesión abierta. */
  public deleteStudent$() {
    return this.api.delete<string>('Student/delete-student', '');
  }

  /** Filtra por nombre, documento, correo o materias; ignora mayúsculas y tildes. */
  public filterStudents(rows: StudentRow[], term: string): StudentRow[] {
    const query = this.normalize(term);
    if (!query) return rows;

    return rows.filter(s =>
      this.normalize(`${s.fullName} ${s.identification} ${s.email ?? ''} ${s.subjectInscription}`).includes(query)
    );
  }

  private normalize(text: string): string {
    return text.normalize('NFD').replace(/\p{Diacritic}/gu, '').toLowerCase().trim();
  }

  // ---------------------------------------------------------------------------
  // Reporte en PDF
  // ---------------------------------------------------------------------------

  /** Descarga el reporte de estudiantes en PDF. Las librerías se cargan solo al usarlo. */
  public async exportPdf(rows: StudentRow[] = this.students()): Promise<void> {

    const [{ jsPDF }, { default: autoTable }] = await Promise.all([
      import('jspdf'),
      import('jspdf-autotable')
    ]);

    // Colores de marca (RGB)
    const NAVY: [number, number, number]        = [20, 18, 58];
    const VIOLET: [number, number, number]      = [91, 61, 245];
    const VIOLET_SOFT: [number, number, number] = [236, 232, 255];
    const CORAL: [number, number, number]       = [255, 122, 89];
    const MINT: [number, number, number]        = [18, 184, 134];
    const TEXT: [number, number, number]        = [23, 24, 59];
    const MUTED: [number, number, number]       = [107, 111, 147];
    const SOFT: [number, number, number]        = [245, 246, 251];
    const LINE: [number, number, number]        = [228, 230, 242];

    const doc = new jsPDF({ orientation: 'landscape', unit: 'pt', format: 'a4' });
    const pageW = doc.internal.pageSize.getWidth();
    const pageH = doc.internal.pageSize.getHeight();
    const margin = 40;
    const now = new Date();

    // Resumen
    const total = rows.length;
    const conMaterias = rows.filter(s => s.credits > 0).length;
    const creditos = rows.reduce((suma, s) => suma + s.credits, 0);
    const completos = rows.filter(s => s.credits >= 9).length;

    // Franja del encabezado (se repite en cada página)
    const dibujarEncabezado = () => {
      doc.setFillColor(...NAVY);
      doc.rect(0, 0, pageW, 78, 'F');
      doc.setFillColor(...CORAL);
      doc.rect(0, 78, pageW, 4, 'F');
      doc.setFillColor(...VIOLET);
      doc.rect(0, 78, pageW * 0.62, 4, 'F');

      // Marca: cuadro redondeado con las iniciales
      doc.setFillColor(...VIOLET);
      doc.roundedRect(margin, 20, 38, 38, 9, 9, 'F');
      doc.setTextColor(255, 255, 255);
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(15);
      doc.text('CN', margin + 19, 44, { align: 'center' });

      doc.setFontSize(19);
      doc.text('Campus Nova', margin + 52, 36);
      doc.setFont('helvetica', 'normal');
      doc.setFontSize(10);
      doc.setTextColor(190, 192, 225);
      doc.text('Reporte de estudiantes del programa de créditos', margin + 52, 52);

      doc.setFontSize(9);
      doc.text(
        `${now.toLocaleDateString('es-CO')}  ·  ${now.toLocaleTimeString('es-CO', { hour: '2-digit', minute: '2-digit' })}`,
        pageW - margin, 36, { align: 'right' }
      );
      doc.setTextColor(255, 255, 255);
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(11);
      doc.text(`${total} estudiantes`, pageW - margin, 52, { align: 'right' });
    };

    // Tarjetas de resumen (solo en la primera página)
    const dibujarTarjetas = () => {
      const tarjetas: { valor: string; etiqueta: string; color: [number, number, number] }[] = [
        { valor: String(total),       etiqueta: 'Estudiantes',          color: VIOLET },
        { valor: String(conMaterias), etiqueta: 'Con materias',         color: CORAL },
        { valor: String(completos),   etiqueta: 'Con cupo completo',    color: MINT },
        { valor: String(creditos),    etiqueta: 'Créditos inscritos',   color: NAVY }
      ];

      const gap = 14;
      const ancho = (pageW - margin * 2 - gap * (tarjetas.length - 1)) / tarjetas.length;
      const y = 104;

      tarjetas.forEach((t, i) => {
        const x = margin + i * (ancho + gap);
        doc.setFillColor(...SOFT);
        doc.setDrawColor(...LINE);
        doc.roundedRect(x, y, ancho, 52, 10, 10, 'FD');
        doc.setFillColor(...t.color);
        doc.roundedRect(x, y, 5, 52, 2, 2, 'F');

        doc.setTextColor(...TEXT);
        doc.setFont('helvetica', 'bold');
        doc.setFontSize(20);
        doc.text(t.valor, x + 20, y + 28);

        doc.setTextColor(...MUTED);
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(9.5);
        doc.text(t.etiqueta, x + 20, y + 43);
      });
    };

    dibujarTarjetas();

    autoTable(doc, {
      startY: 178,
      margin: { top: 100, left: margin, right: margin, bottom: 46 },
      head: [['Estudiante', 'Documento', 'Correo', 'Materias inscritas', 'Créditos']],
      body: rows.map(s => [
        s.isMine ? `${s.fullName}  (Tú)` : s.fullName,
        String(s.identification),
        s.email ?? '',
        s.subjectInscription || 'Sin materias',
        String(s.credits)
      ]),
      styles: {
        font: 'helvetica', fontSize: 9.5, cellPadding: { top: 8, bottom: 8, left: 10, right: 10 },
        textColor: TEXT, lineColor: LINE, lineWidth: 0.4, valign: 'middle'
      },
      headStyles: { fillColor: VIOLET, textColor: 255, fontStyle: 'bold', fontSize: 9.5, lineWidth: 0 },
      alternateRowStyles: { fillColor: SOFT },
      columnStyles: {
        0: { fontStyle: 'bold', cellWidth: 160 },
        1: { cellWidth: 90 },
        2: { cellWidth: 170 },
        4: { halign: 'center', cellWidth: 60, fontStyle: 'bold' }
      },
      didParseCell: (data) => {
        if (data.section !== 'body') return;
        const estudiante = rows[data.row.index];

        // Mi fila resaltada
        if (estudiante?.isMine) {
          data.cell.styles.fillColor = VIOLET_SOFT;
        }

        // Créditos: verde con cupo completo, gris si no tiene materias
        if (data.column.index === 4) {
          if (estudiante.credits >= 9) data.cell.styles.textColor = MINT;
          else if (estudiante.credits === 0) data.cell.styles.textColor = MUTED;
          else data.cell.styles.textColor = VIOLET;
        }

        if (data.column.index === 3 && !estudiante.subjectInscription) {
          data.cell.styles.textColor = MUTED;
          data.cell.styles.fontStyle = 'italic';
        }
      },
      didDrawPage: (data) => {
        dibujarEncabezado();

        // Pie de página
        doc.setDrawColor(...LINE);
        doc.line(margin, pageH - 34, pageW - margin, pageH - 34);
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(8.5);
        doc.setTextColor(...MUTED);
        doc.text('Campus Nova · Registro de estudiantes', margin, pageH - 20);
        doc.text(`Página ${data.pageNumber}`, pageW - margin, pageH - 20, { align: 'right' });
      }
    });

    // Total de páginas en el pie ("Página X de N")
    const paginas = doc.getNumberOfPages();
    for (let i = 1; i <= paginas; i++) {
      doc.setPage(i);
      doc.setFillColor(255, 255, 255);
      doc.rect(pageW - margin - 90, pageH - 30, 90, 14, 'F');
      doc.setFontSize(8.5);
      doc.setTextColor(...MUTED);
      doc.text(`Página ${i} de ${paginas}`, pageW - margin, pageH - 20, { align: 'right' });
    }

    doc.save(`estudiantes-${now.toISOString().slice(0, 10)}.pdf`);
  }

  // ---------------------------------------------------------------------------
  // Formulario
  // ---------------------------------------------------------------------------

  public createForm() {
    return this.fb.nonNullable.group({
      firstName:       ['', [Validators.required, Validators.maxLength(100)]],
      lastName:        ['', [Validators.required, Validators.maxLength(100)]],
      email:           ['', [Validators.required, Validators.email, Validators.maxLength(120)]],
      identification:  ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      password:        [''],
      passwordConfirm: ['']
    }, { validators: passwordsMatch });
  }

  /** Llena el formulario y ajusta qué campos se pueden editar según el modo. */
  public fillForm(form: ReturnType<StudentsService['createForm']>, modo: ModoPanel, student: StudentRow | null): void {
    const partes = (student?.fullName ?? '').split(' ');

    form.reset({
      firstName: partes[0] ?? '',
      lastName: partes.slice(1).join(' '),
      email: student?.email ?? '',
      identification: student ? String(student.identification) : '',
      password: '',
      passwordConfirm: ''
    });

    const password = form.controls.password;
    password.setValidators([strongPassword]);
    password.updateValueAndValidity();

    form.enable();
    if (modo === 'ver') form.disable();
    if (modo === 'editar') form.controls.identification.disable();
  }

  // ---------------------------------------------------------------------------
  // Transformaciones para mostrar
  // ---------------------------------------------------------------------------

  /** "Camila Torres" -> "CT" */
  public iniciales(fullName: string): string {
    const partes = fullName.trim().split(/\s+/);
    return `${partes[0]?.[0] ?? ''}${partes[1]?.[0] ?? ''}`.toUpperCase();
  }

  /** El backend manda las materias en un solo texto: "Cálculo I, Bases de Datos". */
  public materiasDe(student: StudentRow | null): string[] {
    return student?.subjectInscription ? student.subjectInscription.split(', ').filter(Boolean) : [];
  }

  public tonoDe(indice: number): string {
    return 'tono-' + TONOS[indice % TONOS.length];
  }
}
