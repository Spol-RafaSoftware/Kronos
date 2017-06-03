FROM microsoft/dotnet:runtime

# Required to correct .NET Core startup
WORKDIR /app

# maintener info
MAINTAINER Lukasz Pyrzyk <lukasz.pyrzyk@gmail.com>

# copy published binaries to the container
COPY ./Src/Kronos.Server/bin/ /app

# set entrypoint to the docker run
ENTRYPOINT ["dotnet", "Kronos.Server.dll"]
