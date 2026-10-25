#!/bin/bash
# Crea CampusNovaDB y carga los scripts 01..06 solo si la base aun no tiene datos.
set -e

SQLCMD="/opt/mssql-tools18/bin/sqlcmd -S db -U sa -P ${MSSQL_SA_PASSWORD} -C -f 65001 -b"

echo "Esperando a SQL Server..."
until $SQLCMD -Q "SELECT 1" >/dev/null 2>&1; do sleep 2; done

if $SQLCMD -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM CampusNovaDB.academic.subject" 2>/dev/null | grep -qx "10"; then
    echo "CampusNovaDB ya esta inicializada. Nada que hacer."
    exit 0
fi

for f in /scripts/0*.sql; do
    echo "Ejecutando $(basename "$f")"
    $SQLCMD -i "$f"
done
echo "Base de datos lista."
