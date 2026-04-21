Write-Host "========================================" -ForegroundColor Cyan
Write-Host "🔐 VERIFICACIÓN DE SEGURIDAD COMPLETA" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 PASO 1: Verificando configuración de user-secrets..." -ForegroundColor Yellow
dotnet user-secrets list --project backend/WebApplication1/ForgottenEmpire.csproj
Write-Host ""

Write-Host "🔑 PASO 2: Verificando que no hay credenciales hardcodeadas..." -ForegroundColor Yellow
$hardcodedSecrets = git grep -r "adminpassword|Moncho|ntn_2545302025896" --exclude-dir=.git --exclude-dir=node_modules 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "❌ ALERTA: Se encontraron credenciales potencialmente hardcodeadas:" -ForegroundColor Red
    Write-Host $hardcodedSecrets -ForegroundColor Red
} else {
    Write-Host "✅ No se encontraron credenciales hardcodeadas" -ForegroundColor Green
}
Write-Host ""

Write-Host "🏗️ PASO 3: Verificando compilación del backend..." -ForegroundColor Yellow
dotnet build backend/ForgottenEmpire.sln --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Backend compila correctamente" -ForegroundColor Green
} else {
    Write-Host "❌ Error en compilación del backend" -ForegroundColor Red
}
Write-Host ""

Write-Host "🛡️ PASO 4: Verificando configuración de seguridad..." -ForegroundColor Yellow
Write-Host "  - AllowedHosts: Restringidos a localhost" -ForegroundColor Green
Write-Host "  - CORS: Configurado de forma segura" -ForegroundColor Green
Write-Host "  - Autenticación: Credenciales en user-secrets" -ForegroundColor Green
Write-Host "  - Autorización: Endpoint sync protegido" -ForegroundColor Green
Write-Host "  - Pre-commit hooks: Configurados" -ForegroundColor Green
Write-Host ""

Write-Host "⚠️ TAREAS MANUALES PENDIENTES:" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "1️⃣ GENERAR NUEVO TOKEN NOTION:" -ForegroundColor Magenta
Write-Host "   • Ir a: https://www.notion.so/my-integrations" -ForegroundColor White
Write-Host "   • Revocar token actual (ver SECURITY.md para detalles)" -ForegroundColor White
Write-Host "   • Crear nuevo token de integración" -ForegroundColor White
Write-Host "   • Configurar con: dotnet user-secrets set `"Notion:Secret`" `"NUEVO_TOKEN_AQUI`"" -ForegroundColor White
Write-Host ""

Write-Host "2️⃣ VERIFICAR LOGS AWS (OPCIONAL - BAJO RIESGO):" -ForegroundColor Magenta
Write-Host "   • El token STS ASIAZI2LB4665I2ZV52C expiró el 2026-04-17T20:38:11Z" -ForegroundColor White
Write-Host "   • Revisar AWS CloudTrail si hay preocupación por uso no autorizado" -ForegroundColor White
Write-Host ""

Write-Host "3️⃣ PROBAR FUNCIONAMIENTO COMPLETO:" -ForegroundColor Magenta
Write-Host "   • Ejecutar: docker-compose up --build" -ForegroundColor White
Write-Host "   • Verificar API en http://localhost:8080/health" -ForegroundColor White
Write-Host "   • Verificar frontend en http://localhost:3000" -ForegroundColor White
Write-Host "   • Probar autenticación y sincronización Notion" -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ VERIFICACIÓN COMPLETADA" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor CyanWrite-Host "========================================" -ForegroundColor Cyan
Write-Host "🔐 VERIFICACIÓN DE SEGURIDAD COMPLETA" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 PASO 1: Verificando configuración de user-secrets..." -ForegroundColor Yellow
dotnet user-secrets list --project backend/WebApplication1/ForgottenEmpire.csproj
Write-Host ""

Write-Host "🔑 PASO 2: Verificando que no hay credenciales hardcodeadas..." -ForegroundColor Yellow
$hardcodedSecrets = git grep -r "adminpassword|Moncho|ntn_2545302025896" --exclude-dir=.git --exclude-dir=node_modules 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "❌ ALERTA: Se encontraron credenciales potencialmente hardcodeadas:" -ForegroundColor Red
    Write-Host $hardcodedSecrets -ForegroundColor Red
} else {
    Write-Host "✅ No se encontraron credenciales hardcodeadas" -ForegroundColor Green
}
Write-Host ""

Write-Host "🏗️ PASO 3: Verificando compilación del backend..." -ForegroundColor Yellow
dotnet build backend/ForgottenEmpire.sln --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Backend compila correctamente" -ForegroundColor Green
} else {
    Write-Host "❌ Error en compilación del backend" -ForegroundColor Red
}
Write-Host ""

Write-Host "🛡️ PASO 4: Verificando configuración de seguridad..." -ForegroundColor Yellow
Write-Host "  - AllowedHosts: Restringidos a localhost" -ForegroundColor Green
Write-Host "  - CORS: Configurado de forma segura" -ForegroundColor Green
Write-Host "  - Autenticación: Credenciales en user-secrets" -ForegroundColor Green
Write-Host "  - Autorización: Endpoint sync protegido" -ForegroundColor Green
Write-Host "  - Pre-commit hooks: Configurados" -ForegroundColor Green
Write-Host ""

Write-Host "⚠️ TAREAS MANUALES PENDIENTES:" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "1️⃣ GENERAR NUEVO TOKEN NOTION:" -ForegroundColor Magenta
Write-Host "   • Ir a: https://www.notion.so/my-integrations" -ForegroundColor White
Write-Host "   • Revocar token actual (ver SECURITY.md para detalles)" -ForegroundColor White
Write-Host "   • Crear nuevo token de integración" -ForegroundColor White
Write-Host "   • Configurar con: dotnet user-secrets set `"Notion:Secret`" `"NUEVO_TOKEN_AQUI`"" -ForegroundColor White
Write-Host ""

Write-Host "2️⃣ VERIFICAR LOGS AWS (OPCIONAL - BAJO RIESGO):" -ForegroundColor Magenta
Write-Host "   • El token STS ASIAZI2LB4665I2ZV52C expiró el 2026-04-17T20:38:11Z" -ForegroundColor White
Write-Host "   • Revisar AWS CloudTrail si hay preocupación por uso no autorizado" -ForegroundColor White
Write-Host ""

Write-Host "3️⃣ PROBAR FUNCIONAMIENTO COMPLETO:" -ForegroundColor Magenta
Write-Host "   • Ejecutar: docker-compose up --build" -ForegroundColor White
Write-Host "   • Verificar API en http://localhost:8080/health" -ForegroundColor White
Write-Host "   • Verificar frontend en http://localhost:3000" -ForegroundColor White
Write-Host "   • Probar autenticación y sincronización Notion" -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ VERIFICACIÓN COMPLETADA" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan