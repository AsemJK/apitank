# API TANK

## Overview
In this project you will seed how redis can be used as a caching layer for an API. The API build using Asp.net core
The data is mock data and is stored in a List<T> object. The data is fetched from the list and returned to the user. 
The data is cached in redis for 5 minutes. If the data is requested again within 5 minutes, it is returned from the cache instead of fetching it from the list.

- [Using Docker File](#using_docker_file)
- [Using Dotnet CLI](#using_dotnet_cli)

## Using Docker File
To run the project, you need to have Docker installed on your machine. You can download it from [here](https://www.docker.com/products/docker-desktop).
1. Clone the repository
2. Open a terminal and navigate to the project directory
3. Run the following command to build and run the containers:
   ```bash
   docker-compose up --build
   ```
4. The API will be available at `http://localhost:8003` and Redis will be available at `127.0.0.1:6379`.

## Using Dotnet CLI
1. Open a terminal and navigate to the project directory
2. Type the following command to build the docker image
   ```bash
   dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer -c Release
   ```
This will create docker image with the name `apitank:latest`
3. Type the following command to run the docker image
   ```bash
   docker run -d -p 8003:80 --name apitank --network="host" apitank:latest
   ```
Note: You can put the configuration in .csproj file to avoid typing the long command every time.
```
    <PropertyGroup>
        <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>
        <DockerDefaultTargetArchitecture>x64</DockerDefaultTargetArchitecture>
        <PublishProfile>DefaultContainer</PublishProfile>
        <ContainerImageName>apitank</ContainerImageName>
        <ContainerRegistry>local</ContainerRegistry>
        <ContainerBuildArgs></ContainerBuildArgs>
        <ContainerFamily>jammy-chiseled</ContainerFamily>
    </PropertyGroup>
```
### Explanation of the properties used in .csproj file
- DockerDefaultTargetOS: Specifies the target operating system for the Docker image. In this case, it's set to Linux.
- ContainerFamily is an MSBuild property used when publishing or building container images directly from your project (without writing a Dockerfile).
It tells the .NET SDK which base container image family to use when it generates the container.
jammy → refers to Ubuntu 22.04 (Jammy Jellyfish).
chiseled → refers to Microsoft’s chiseled Ubuntu images, which are **very slim**, minimal images hardened for production.
(Much smaller than full Ubuntu, great for security and performance.)
