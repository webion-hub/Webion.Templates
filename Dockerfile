ARG sdk_version=6.0-alpine
ARG project_dir=Webion.Templates.Api

FROM mcr.microsoft.com/dotnet/sdk:$sdk_version AS build

WORKDIR /build
COPY ./ .
RUN dotnet restore

WORKDIR /build/$project_dir
RUN dotnet publish -c release -o /release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:$sdk_version
WORKDIR /release
COPY --from=build /release ./

ENTRYPOINT [ "dotnet", "Webion.Templates.Api.dll" ]