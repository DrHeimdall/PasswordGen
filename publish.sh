dotnet publish -nologo -c release --self-contained true --runtime linux-x64
# dotnet publish -nologo -c release --self-contained true --runtime linux-musl-x64
dotnet publish -nologo -c release --self-contained true --runtime linux-arm
# dotnet publish -nologo -c release --self-contained true --runtime linux-arm64
# dotnet publish -nologo -c release --self-contained true --runtime rhel-x64

dotnet publish -nologo -c release --self-contained true --runtime osx-x64

dotnet publish -nologo -c release --self-contained true --runtime win-x64
# dotnet publish -nologo -c release --self-contained true --runtime win-x86
# dotnet publish -nologo -c release --self-contained true --runtime win-arm
# dotnet publish -nologo -c release --self-contained true --runtime win-arm64


dotnet publish -nologo -c release --self-contained false --runtime linux-x64
# dotnet publish -nologo -c release --self-contained false --runtime linux-musl-x64
dotnet publish -nologo -c release --self-contained false --runtime linux-arm
# dotnet publish -nologo -c release --self-contained false --runtime linux-arm64
# dotnet publish -nologo -c release --self-contained false --runtime rhel-x64

dotnet publish -nologo -c release --self-contained false --runtime osx-x64

dotnet publish -nologo -c release --self-contained false --runtime win-x64
# dotnet publish -nologo -c release --self-contained false --runtime win-x86
# dotnet publish -nologo -c release --self-contained false --runtime win-arm
# dotnet publish -nologo -c release --self-contained false --runtime win-arm64
