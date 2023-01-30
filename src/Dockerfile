FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN addgroup --system --gid 1000 dotnetgroup \
    && adduser --system --uid 1000 --ingroup dotnetgroup --shell /bin/sh dotnet
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 3600
ENV ASPNETCORE_URLS=http://+:3600

USER 1000
ENTRYPOINT ["dotnet", "svc1.dll"]