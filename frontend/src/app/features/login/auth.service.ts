import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { ApiService } from "../../core/services/http-client/api.service";
import { AlertService } from "../../core/services/alert/alert.service";
import { StorageService } from "../../core/services/storage/storage.service";
import { ResponseStatus } from "../../core/enums/response-status";
import { session } from "../../core/entities/session.entities";
import { LoginFormValue, LoginRequest } from "./login.entity";

@Injectable({ providedIn: 'root' })
export class AuthService {

    private readonly api = inject(ApiService);
    private readonly router = inject(Router);
    private readonly alert = inject(AlertService);
    private readonly storage = inject(StorageService);

    /**
     * El backend responde con las cookies HttpOnly access_token y refresh_token.
     * Aquí solo se guardan los datos de sesión que la UI necesita mostrar
     * (nombre, rol, instituto); el token nunca es accesible desde JavaScript.
     */
    public login(form: LoginFormValue, onDone?: () => void) {
        const credentials: LoginRequest = {
            institutionCode: Number(form.institutionCode),
            username: Number(form.username),
            password: form.password
        };

        return this.api.add<session, LoginRequest>('User/login-access', credentials).subscribe({
            next: async (res) => {
                if (res.status !== ResponseStatus.Success) {
                    onDone?.();
                    await this.alert.error("No pudimos entrar", res.message);
                    return;
                }

                await this.storage.set<session | null>("dta_session", res.data);
                await this.storage.set<string>(
                    "dta_active_session",
                    JSON.stringify({ "sesssion_active": true })
                );

                onDone?.();
                await this.alert.success("¡Adentro!", "Sesión iniciada correctamente.");
                this.router.navigate(["pages/subjects"]);
            },
            error: async (err) => {
                onDone?.();
                const message = err?.error?.message
                    ?? "Revisa el código institucional, el documento y la contraseña.";
                await this.alert.error("No pudimos entrar", message);
            }
        });
    }
}
