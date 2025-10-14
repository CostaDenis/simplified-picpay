FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY simplified-picpay.csproj . 
RUN dotnet restore "./simplified-picpay.csproj"
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "simplified_picpay.dll" ]