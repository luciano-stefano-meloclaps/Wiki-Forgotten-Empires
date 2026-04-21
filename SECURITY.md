# 🔒 SEGURIDAD - Guía de Configuración

## 🚨 CONFIGURACIÓN INICIAL REQUERIDA

### 1. Configurar Credenciales Seguras

```bash
# JWT Secret (generar uno nuevo de 64+ caracteres)
dotnet user-secrets set "Authentication:SecretForKey" "tu-jwt-secret-super-seguro-aqui"

# Notion Integration Token (generar nuevo en https://www.notion.so/my-integrations)
dotnet user-secrets set "Notion:Secret" "ntn_nuevo-token-de-notion-aqui"

# Credenciales de desarrollo (temporal - reemplazar por BD en producción)
dotnet user-secrets set "Authentication:AdminUsername" "admin"
dotnet user-secrets set "Authentication:AdminPassword" "password-seguro"
```

### 2. Variables de Entorno

Copiar `.env.example` a `.env.local` y configurar:

```bash
cp .env.example .env.local
# Editar .env.local con valores reales
```

### 3. Verificar Configuración

```bash
# Ver todas las secrets configuradas
dotnet user-secrets list

# Verificar que no hay secrets hardcodeados
git grep -r "adminpassword\|Moncho\|ntn_2545302025896" --exclude-dir=.git
```

## 🛡️ MEDIDAS DE SEGURIDAD IMPLEMENTADAS

### ✅ Autenticación
- Credenciales movidas a user-secrets (no hardcodeadas)
- JWT tokens con expiración de 1 hora
- Endpoint `/sync` requiere rol Admin

### ✅ CORS
- Orígenes configurables desde appsettings
- Métodos específicos (no AllowAnyMethod)
- Headers específicos (no AllowAnyHeader)

### ✅ Headers de Seguridad
- AllowedHosts restringidos (no "*")
- Host Header Injection prevention

### ✅ Git Hooks
- Pre-commit detection de secrets con secretlint
- Prevención de commits con credenciales expuestas

## 🚫 PROHIBIDO

- ❌ Nunca commitear `appsettings.Development.json`
- ❌ Nunca hardcodear credenciales en código
- ❌ Nunca usar `AllowedHosts: "*"` en producción
- ❌ Nunca exponer stack traces en respuestas de error
- ❌ Nunca commitear archivos `.env.local`

## 🔍 AUDITORÍA DE SEGURIDAD

Ejecutar auditoría completa:

```bash
# Buscar posibles secrets expuestos
git grep -r "password\|secret\|token\|key" --exclude-dir=.git

# Verificar configuración de secrets
dotnet user-secrets list

# Ejecutar secretlint manualmente
npx secretlint "**/*.{js,ts,cs,json}"
```

## 📞 CONTACTO

Si encuentras vulnerabilidades de seguridad, reportar inmediatamente al equipo de desarrollo.