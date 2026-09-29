# Playwright Automation — Automation Exercise

## Descripción

Framework de pruebas E2E sobre el sitio de práctica [automationexercise.com](https://automationexercise.com/), construido con **Playwright for .NET + NUnit** y el patrón **Page Object Model**.

Cubre 5 funcionalidades con 15 pruebas funcionales:

| Funcionalidad | Clase de prueba | Pruebas |
|---|---|---|
| Login | `Tests/Authentication/LoginTests.cs` | Login válido · contraseña incorrecta · campos obligatorios vacíos |
| Registro / cuenta | `Tests/Authentication/RegistrationTests.cs` | Alta de usuario nuevo · email ya registrado · eliminación de cuenta |
| Productos | `Tests/Products/ProductTests.cs` | Catálogo · búsqueda · detalle de producto |
| Carrito | `Tests/Cart/CartTests.cs` | Agregar un producto · agregar varios · eliminar producto |
| Checkout | `Tests/Checkout/CheckoutTests.cs` | Acceso al checkout · resumen de compra · compra completa |

## Stack

```text
C#
.NET 8
Playwright for .NET (Microsoft.Playwright.NUnit)
NUnit
Page Object Model
Graphify
```

## Requisitos

```text
.NET 8 SDK (o un SDK más reciente con el runtime de .NET 8 instalado)
PowerShell / pwsh
Python 3.10+
uv o pipx (o pip) para Graphify
```

## Instalación

```powershell
dotnet restore
dotnet build
pwsh PlaywrightAutomation.Tests/bin/Debug/net8.0/playwright.ps1 install
```

Si `pwsh` no está instalado, Windows PowerShell funciona igual:

```powershell
powershell -ExecutionPolicy Bypass -File PlaywrightAutomation.Tests/bin/Debug/net8.0/playwright.ps1 install
```

## Ejecución

Todas las pruebas (Chromium por defecto):

```powershell
dotnet test
```

Por clase, por nombre o por categoría:

```powershell
dotnet test --filter "FullyQualifiedName~LoginTests"
dotnet test --filter "Name~Login_WithValidCredentials"
dotnet test --filter "Category=Smoke"
```

Salida detallada:

```powershell
dotnet test --logger "console;verbosity=detailed"
```

## Configuración

Toda la configuración vive en archivos `.runsettings` dentro de `PlaywrightAutomation.Tests/`. Hay dos perfiles:

| Perfil | Archivo | Uso |
|---|---|---|
| Por defecto | `playwright.runsettings` | Headless, evidencias solo ante fallo. Se aplica automáticamente con `dotnet test`. |
| Debug | `debug.runsettings` | Navegador visible, `SlowMo` de 300 ms, trace y video de **todas** las pruebas. |

```powershell
dotnet test -s PlaywrightAutomation.Tests/debug.runsettings
```

### Opciones del navegador (sección `<Playwright>`)

Las gestiona Microsoft.Playwright.NUnit de forma nativa. Se sobrescriben en la línea de comandos después de `--`:

| Opción | Valores | Por defecto | Ejemplo |
|---|---|---|---|
| `BrowserName` | `chromium`, `firefox`, `webkit` | `chromium` | `dotnet test -- Playwright.BrowserName=firefox` |
| `LaunchOptions.Headless` | `true`, `false` | `true` | `dotnet test -- Playwright.LaunchOptions.Headless=false` |
| `LaunchOptions.SlowMo` | ms entre acciones | `0` | `dotnet test -- Playwright.LaunchOptions.SlowMo=500` |
| `LaunchOptions.Channel` | `chrome`, `msedge` | (Chromium incluido) | `dotnet test -- Playwright.LaunchOptions.Channel=msedge` |
| `ExpectTimeout` | ms para `Expect(...)` | `10000` | `dotnet test -- Playwright.ExpectTimeout=15000` |

El navegador también se puede elegir con la variable de entorno `BROWSER`, que es nativa de Playwright.

### Opciones del framework (sección `<TestRunParameters>`)

Las lee `Configuration/TestSettings.cs`. Cada una se puede sobrescribir con una variable de entorno `E2E_*`, que tiene prioridad sobre el archivo:

| Parámetro | Variable de entorno | Valores | Por defecto |
|---|---|---|---|
| `BaseUrl` | `E2E_BASE_URL` | URL | `https://automationexercise.com` |
| `ViewportWidth` / `ViewportHeight` | `E2E_VIEWPORT_WIDTH` / `E2E_VIEWPORT_HEIGHT` | px | `1366` / `900` |
| `DefaultTimeout` | `E2E_DEFAULT_TIMEOUT` | ms para acciones y navegación | `30000` |
| `BlockAds` | `E2E_BLOCK_ADS` | `true`, `false` | `true` |
| `Screenshot` | `E2E_SCREENSHOT` | `off`, `on-failure`, `always` | `on-failure` |
| `Trace` | `E2E_TRACE` | `off`, `on-failure`, `always` | `on-failure` |
| `Video` | `E2E_VIDEO` | `off`, `on-failure`, `always` | `off` |
| `ArtifactsDirectory` | `E2E_ARTIFACTS_DIRECTORY` | ruta absoluta o relativa a `bin/Debug/net8.0` | `playwright-artifacts` |

```powershell
$env:E2E_VIDEO = "on-failure"; $env:E2E_TRACE = "always"; dotnet test
```

Un valor inválido (por ejemplo `E2E_TRACE=sometimes`) hace fallar la ejecución con un mensaje que indica los valores aceptados, en lugar de ignorarse en silencio.

La paralelización (`NumberOfTestWorkers`) está fijada en **1** a propósito: el sitio es público y compartido, y se prioriza la estabilidad.

### Reportes

Cada ejecución de `dotnet test` genera, en `TestResults/` en la raíz de la solución:

| Archivo | Contenido | Cómo abrirlo |
|---|---|---|
| `TestReport.html` | Resumen (total, pasadas, fallidas, % y duración) y detalle de cada fallo con el mensaje de Playwright y el stack trace. | Doble clic, en cualquier navegador. |
| `TestResults.trx` | Resultado completo en formato Visual Studio, con enlaces a las evidencias adjuntas (screenshot, trace, video). | Visual Studio, Azure DevOps o GitHub Actions (por ejemplo, con `dorny/test-reporter`). |

El reporte de la última ejecución reemplaza al anterior. Las evidencias adjuntas se copian en una subcarpeta con fecha y hora dentro de `TestResults/`. Los loggers se configuran en la sección `<LoggerRunSettings>` de cada `.runsettings`.

### Evidencias

Según la configuración, `BaseTest` guarda en `bin/Debug/net8.0/playwright-artifacts/` archivos con el nombre `<Test>.<navegador>`:

- `.png`: screenshot de página completa.
- `.trace.zip`: trace de Playwright (acciones, DOM, red, screenshots). Ábrelo en [trace.playwright.dev](https://trace.playwright.dev) o con `pwsh bin/Debug/net8.0/playwright.ps1 show-trace <archivo>`.
- `.webm`: video de la prueba.

Los archivos se adjuntan al resultado de NUnit. Con la configuración por defecto, las pruebas que pasan no generan evidencias.

## Estructura

```text
PlaywrightAutomation/
├── PlaywrightAutomation.sln
├── .graphifyignore                  # excluye bin/obj del análisis de Graphify
└── PlaywrightAutomation.Tests/
    ├── Pages/                       # Page Objects: locators + acciones, sin assertions de negocio
    │   ├── Components/
    │   │   ├── HeaderComponent.cs   # barra de navegación común
    │   │   └── OrderItemsTable.cs   # tabla de productos compartida por Cart y Checkout
    │   ├── LoginPage.cs
    │   ├── RegisterPage.cs
    │   ├── ProductsPage.cs
    │   ├── ProductDetailsPage.cs
    │   ├── CartPage.cs
    │   ├── CheckoutPage.cs
    │   └── PaymentPage.cs
    ├── Tests/                       # escenarios agrupados por funcionalidad
    ├── Configuration/
    │   └── TestSettings.cs          # lee TestRunParameters + variables de entorno E2E_*
    ├── Fixtures/
    │   ├── BaseTest.cs              # contexto, bloqueo de anuncios, trace/screenshot, Page Objects, limpieza de usuarios
    │   └── AuthenticatedTest.cs     # crea un usuario nuevo e inicia sesión antes de cada prueba
    ├── TestData/                    # datos estáticos: productos conocidos, mensajes esperados, tarjeta de prueba
    ├── Models/                      # TestUser, Product, PaymentCard
    ├── Utilities/
    │   ├── TestDataGenerator.cs     # usuarios con email único por ejecución
    │   └── AccountApiClient.cs      # alta/baja de cuentas vía API del sitio
    ├── playwright.runsettings       # perfil por defecto
    └── debug.runsettings            # perfil de depuración
```

Dirección de dependencias: `Tests → Fixtures → Pages → Components/Models`, y `Fixtures → Utilities / Configuration`. Ningún Page Object depende de Tests ni de Fixtures.

## Decisiones de diseño

- **Selectores.** Se prioriza `GetByRole`, `GetByPlaceholder`, `GetByLabel` y `GetByText`. El sitio expone atributos `data-qa`, así que se configuran como test id (`Selectors.SetTestIdAttribute("data-qa")`) y se usan con `GetByTestId` donde el rol o el texto son ambiguos. Por ejemplo, los formularios de login y signup comparten el placeholder "Email Address". CSS se usa solo para contenedores sin semántica accesible, como la tabla del carrito o el botón de búsqueda sin nombre accesible.
- **Independencia.** Cada prueba corre en un contexto de navegador nuevo y crea sus propios datos. Los usuarios se crean con la API pública del sitio (`/api/createAccount`), que es rápida y no depende de la UI de registro, y se eliminan en el `TearDown` (`/api/deleteAccount`). No hay `[Order]` ni estado compartido.
- **Sincronización.** No se usa `Thread.Sleep`. Todo se basa en el auto-waiting de Playwright y en `Expect(...)`. En el modal "Added!" se espera a que aparezca y desaparezca, que es una condición real de la UI.
- **Anuncios (workaround documentado).** El sitio carga anuncios de Google, incluido el "vignette" a pantalla completa que intercepta clics, además de un banner de consentimiento. `BaseTest` aborta las peticiones a esos dominios de terceros (`googlesyndication`, `doubleclick`, `fundingchoicesmessages`, etc.) con `Context.RouteAsync`. Solo se bloquea contenido externo: las peticiones a `automationexercise.com` no se tocan, por lo que los defectos reales de la aplicación siguen detectándose.
- **Datos de pago.** Se usa el número de tarjeta de prueba público `4111 1111 1111 1111` con datos ficticios. El sitio de práctica no procesa pagos reales.

## Graphify

[Graphify](https://pypi.org/project/graphifyy/) genera un grafo de conocimiento del código (clases, métodos, herencias, referencias) para analizar la arquitectura antes y después de cada cambio.

Instalación:

```powershell
uv tool install graphifyy     # o: pipx install graphifyy / python -m pip install --user graphifyy
graphify install              # instala el skill /graphify para Claude Code
```

Generar o actualizar el grafo desde la raíz de la solución:

- Dentro de Claude Code: `/graphify .`
- Desde PowerShell, solo extracción de código y sin LLM:

  ```powershell
  graphify update .
  ```

Resultado en `graphify-out/`:

```text
graphify-out/
├── graph.html        # visualización interactiva
├── GRAPH_REPORT.md   # god nodes, comunidades, ciclos, nodos aislados
└── graph.json
```

Consultas útiles:

```powershell
graphify affected "LoginPage"          # qué se ve afectado si cambia LoginPage
graphify explain "OrderItemsTable"     # vecinos y relaciones de un nodo
graphify path "CheckoutTests" "OrderItemsTable"
graphify god-nodes
```

Si los resultados incluyen archivos de `bin/` u `obj/`, elimina `graphify-out/` y regenera el grafo: la caché puede conservar nodos de ejecuciones anteriores a `.graphifyignore`.
