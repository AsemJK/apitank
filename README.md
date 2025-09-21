# API TANK

## Overview
In this project you will seed how redis can be used as a caching layer for an API. The API build using Asp.net core
The data is mock data and is stored in a List<T> object. The data is fetched from the list and returned to the user. 
The data is cached in redis for 5 minutes. If the data is requested again within 5 minutes, it is returned from the cache instead of fetching it from the list.

### Docker
To run the project, you need to have Docker installed on your machine. You can download it from [here](https://www.docker.com/products/docker-desktop).
1. Clone the repository
2. Open a terminal and navigate to the project directory
3. Run the following command to build and run the containers:
   ```bash
   docker-compose up --build
   ```
4. The API will be available at `http://localhost:8003` and Redis will be available at `127.0.0.1:6379`.

## Another way to run the project as a container on docker using cmd
1. Open a terminal and navigate to the project directory
1. Type the following command to build the docker image
   ```bash
   dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer -c Release
   ```
This will create docker image with the name `apitank:latest`
1. Type the following command to run the docker image
   ```bash
   docker run -d -p 8003:80 --name apitank --network="host" apitank:latest
   ```