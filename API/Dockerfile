ARG ARCH=amd64
ARG TAG=8.0-bookworm-slim-$ARCH
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

EXPOSE 80
EXPOSE 443

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:$TAG
WORKDIR /App

COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "API.dll"]