# version de la imagen , el as es un alias,nombre
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#crea una carpeta en la imagen app y me meto en ella, es la ruta de trabajo
WORKDIR /app

#Copia el archivo de proyecto TrabajoAlex.csproj al directorio de trabajo. 
#Este archivo suele contener la información sobre las dependencias y la configuración del proyecto
COPY ../API/ContosoPizza.API.csproj .

#restaura las dependencias del proyecto
#Esto se hace antes de copiar los archivos de la aplicación para aprovechar la caché de capas de Docker y optimizar el proceso de construcción.
RUN dotnet restore

#Copia todos los archivos del directorio actual (donde se encuentra el Dockerfile) al directorio de trabajo en la imagen.
COPY . .

#(dotnet build) para compilar la aplicación en modo Release y guarda los resultados en el directorio "/app/build".
# RUN dotnet build -c Release -o /app/build
RUN dotnet build -o /app/build ./API/ContosoPizza.API.csproj

#Inicia una nueva etapa en la construcción de la imagen, basada en la imagen construida anteriormente (etiquetada como "build").
FROM build AS publish
#ejecutar comandos dentro de la ruta, el publish es para publicar la app en .net
# RUN dotnet publish -c Release -o /app/publish
RUN dotnet publish -o /app/publish ./API/ContosoPizza.API.csproj

#Inicia una nueva etapa en la construcción de la imagen, basada en la imagen oficial de ASP.NET para .NET 6.0.
# Esta será la imagen final que contendrá la aplicación publicada.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

#Establece el directorio de trabajo dentro de la imagen final como "/app"
WORKDIR /app

EXPOSE 80

#Copia los archivos publicados desde la etapa "publish" al directorio de trabajo en la imagen final
COPY --from=publish /app/publish .
#cada vez que ejecuta,es el exe, el programa, ejecuta donde trabajoalex.dll
ENTRYPOINT ["dotnet", "ContosoPizza.API.dll"]
