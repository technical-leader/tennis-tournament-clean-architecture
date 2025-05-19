# Etapa 1: Construcción de la aplicación
# Utiliza la imagen del SDK de .NET 9.0 para construir la aplicación.
# Se especifica --platform=$BUILDPLATFORM para compilaciones cruzadas si es necesario.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src

# Copia el archivo de solución y los archivos .csproj de cada proyecto.
# Esto permite a Docker cachear la capa de restauración de NuGet de manera eficiente.
# Si solo cambian los fuentes pero no los proyectos/dependencias, esta capa se reutiliza.
COPY ["TennisTournament.sln", "."]
COPY ["TennisTournament.API/TennisTournament.API.csproj", "TennisTournament.API/"]
COPY ["TennisTournament.Application/TennisTournament.Application.csproj", "TennisTournament.Application/"]
COPY ["TennisTournament.Domain/TennisTournament.Domain.csproj", "TennisTournament.Domain/"]
COPY ["TennisTournament.Infrastructure/TennisTournament.Infrastructure.csproj", "TennisTournament.Infrastructure/"]
# Los proyectos de prueba no se copian, ya que restauraremos directamente el proyecto API.

# Restaura las dependencias NuGet solo para el proyecto API y sus dependencias de proyecto.
RUN dotnet restore "TennisTournament.API/TennisTournament.API.csproj"

# Copia el resto del código fuente de la solución al contenedor.
# Asegúrate de tener un archivo .dockerignore en la raíz de tu solución
# para excluir archivos/directorios innecesarios (como .git, bin, obj, etc.).
COPY . .

# Construye el proyecto principal (API) en configuración Release.
# Se especifica --no-restore porque ya se hizo un restore global.
WORKDIR "/src/TennisTournament.API"
RUN dotnet build "TennisTournament.API.csproj" -c $configuration --no-restore

# Etapa 2: Publicación de la aplicación
FROM build AS publish
ARG configuration=Release
# Publica la aplicación, optimizada para producción.
# Se especifica --no-build porque ya se compiló en la etapa anterior.
RUN dotnet publish "TennisTournament.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false --no-build

# Etapa 3: Creación de la imagen final
# Utiliza la imagen base de ASP.NET Core 9.0, que es más ligera que la del SDK.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Puerto que la API escuchará dentro del contenedor.
EXPOSE 5000
# Configura ASP.NET Core para escuchar en todas las interfaces en el puerto 5000.
ENV ASPNETCORE_URLS=http://+:5000
# Establece una cadena de conexión predeterminada (puede ser sobrescrita en docker run).
ENV ConnectionStrings__DefaultConnection="Data Source=host.docker.internal,1563;Initial Catalog=TennisTournamentDb;User ID=sa;Password=Belgaum135;Connect Timeout=15;Trust Server Certificate=True;Authentication=SqlPassword;Application Name=TennisTournament;Command Timeout=30"
# Cambia al usuario 'app' no root por seguridad.
USER app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TennisTournament.API.dll"]