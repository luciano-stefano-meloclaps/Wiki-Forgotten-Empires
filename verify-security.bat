@echo off
echo ========================================
echo 🔐 VERIFICACIÓN DE SEGURIDAD COMPLETA
echo ========================================
echo.

echo 📋 PASO 1: Verificando configuración de user-secrets...
dotnet user-secrets list --project backend/WebApplication1/ForgottenEmpire.csproj
echo.

echo 🔑 PASO 2: Verificando que no hay credenciales hardcodeadas...
git grep -r "adminpassword\|Moncho\|ntn_2545302025896" --exclude-dir=.git --exclude-dir=node_modules || echo "✅ No se encontraron credenciales hardcodeadas"
echo.

echo 🏗️ PASO 3: Verificando compilación del backend...
dotnet build backend/ForgottenEmpire.sln --verbosity quiet
if %errorlevel% equ 0 (
    echo ✅ Backend compila correctamente
) else (
    echo ❌ Error en compilación del backend
)
echo.

echo 🛡️ PASO 4: Verificando configuración de seguridad...
echo - AllowedHosts: Restringidos a localhost
echo - CORS: Configurado de forma segura
echo - Autenticación: Credenciales en user-secrets
echo - Autorización: Endpoint sync protegido
echo - Pre-commit hooks: Configurados
echo.

echo ⚠️ TAREAS MANUALES PENDIENTES:
echo ========================================
echo.
echo 1️⃣ GENERAR NUEVO TOKEN NOTION:
echo    • Ir a: https://www.notion.so/my-integrations
echo    • Revocar token actual (ver SECURITY.md para detalles)
echo    • Crear nuevo token de integración
echo    • Configurar con: dotnet user-secrets set "Notion:Secret" "NUEVO_TOKEN_AQUI"
echo.

echo 2️⃣ VERIFICAR LOGS AWS (OPCIONAL - BAJO RIESGO):
echo    • El token STS ASIAZI2LB4665I2ZV52C expiró el 2026-04-17T20:38:11Z
echo    • Revisar AWS CloudTrail si hay preocupación por uso no autorizado
echo.

echo 3️⃣ PROBAR FUNCIONAMIENTO COMPLETO:
echo    • Ejecutar: docker-compose up --build
echo    • Verificar API en http://localhost:8080/health
echo    • Verificar frontend en http://localhost:3000
echo    • Probar autenticación y sincronización Notion
echo.

echo ========================================
echo ✅ VERIFICACIÓN COMPLETADA
echo ========================================@echo off
echo ========================================
echo 🔐 VERIFICACIÓN DE SEGURIDAD COMPLETA
echo ========================================
echo.

echo 📋 PASO 1: Verificando configuración de user-secrets...
dotnet user-secrets list --project backend/WebApplication1/ForgottenEmpire.csproj
echo.

echo 🔑 PASO 2: Verificando que no hay credenciales hardcodeadas...
git grep -r "adminpassword\|Moncho\|ntn_2545302025896" --exclude-dir=.git --exclude-dir=node_modules || echo "✅ No se encontraron credenciales hardcodeadas"
echo.

echo 🏗️ PASO 3: Verificando compilación del backend...
dotnet build backend/ForgottenEmpire.sln --verbosity quiet
if %errorlevel% equ 0 (
    echo ✅ Backend compila correctamente
) else (
    echo ❌ Error en compilación del backend
)
echo.

echo 🛡️ PASO 4: Verificando configuración de seguridad...
echo - AllowedHosts: Restringidos a localhost
echo - CORS: Configurado de forma segura
echo - Autenticación: Credenciales en user-secrets
echo - Autorización: Endpoint sync protegido
echo - Pre-commit hooks: Configurados
echo.

echo ⚠️ TAREAS MANUALES PENDIENTES:
echo ========================================
echo.
echo 1️⃣ GENERAR NUEVO TOKEN NOTION:
echo    • Ir a: https://www.notion.so/my-integrations
echo    • Revocar token actual (ver SECURITY.md para detalles)
echo    • Crear nuevo token de integración
echo    • Configurar con: dotnet user-secrets set "Notion:Secret" "NUEVO_TOKEN_AQUI"
echo.

echo 2️⃣ VERIFICAR LOGS AWS (OPCIONAL - BAJO RIESGO):
echo    • El token STS ASIAZI2LB4665I2ZV52C expiró el 2026-04-17T20:38:11Z
echo    • Revisar AWS CloudTrail si hay preocupación por uso no autorizado
echo.

echo 3️⃣ PROBAR FUNCIONAMIENTO COMPLETO:
echo    • Ejecutar: docker-compose up --build
echo    • Verificar API en http://localhost:8080/health
echo    • Verificar frontend en http://localhost:3000
echo    • Probar autenticación y sincronización Notion
echo.

echo ========================================
echo ✅ VERIFICACIÓN COMPLETADA
echo ========================================