FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first so the package layer is cached until a project file changes.
COPY TextileShop.sln ./
COPY src/TextileShop.Domain/TextileShop.Domain.csproj src/TextileShop.Domain/
COPY src/TextileShop.Services/TextileShop.Services.csproj src/TextileShop.Services/
COPY src/TextileShop.Infrastructure/TextileShop.Infrastructure.csproj src/TextileShop.Infrastructure/
COPY src/TextileShop.Web/TextileShop.Web.csproj src/TextileShop.Web/
RUN dotnet restore src/TextileShop.Web/TextileShop.Web.csproj

COPY src/ src/
RUN dotnet publish src/TextileShop.Web/TextileShop.Web.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000
ENTRYPOINT ["/docker-entrypoint.sh"]
