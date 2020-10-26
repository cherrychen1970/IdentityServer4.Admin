# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

RUN git clone https://github.com/cherrychen1970/id4.git
RUN dotnet restore "id4/id4.csproj"
RUN dotnet build "id4/id4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "id4/id4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

ENTRYPOINT ["dotnet", "id4.dll"]
