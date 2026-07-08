FROM ubuntu:resolute AS backend-build

RUN apt-get update && apt-get install -y software-properties-common wget && \
    add-apt-repository ppa:dotnet/backports && \
    apt-get update && apt-get install -y dotnet-sdk-8.0 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY backend/*.csproj .
RUN dotnet restore
COPY backend/ .
RUN dotnet publish -c Release -o /app/publish

FROM ubuntu:resolute AS frontend-build

RUN apt-get update && apt-get install -y nodejs npm && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY frontend/package*.json .
RUN npm ci
COPY frontend/ .
RUN npm run build

FROM ubuntu:resolute AS runtime

RUN apt-get update && apt-get install -y software-properties-common && \
    add-apt-repository ppa:dotnet/backports && \
    apt-get update && apt-get install -y aspnetcore-runtime-8.0 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=backend-build /app/publish .
COPY --from=frontend-build /src/dist wwwroot
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Sugma.Api.dll"]
