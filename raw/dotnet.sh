#!/usr/bin/env bash
# sudo curl -sSL https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/raw/dotnet.sh | bash
clear

addLineToFile() {
    local lineToAdd=$1
    local targetFile=$2

    if [ -z "$lineToAdd" ]; then
        echo "No line provided to add."
        return 1
    fi

    if [ -z "$targetFile" ]; then
        echo "No target file specified."
        return 1
    fi

    if ! grep -qF -- "$lineToAdd" "$targetFile"; then
        echo "$lineToAdd" >> "$targetFile"
        echo "Line $lineToAdd added to $targetFile"
    else
        echo "Line $lineToAdd already present in $targetFile"
    fi
}

doesCommandExist() {
    command -v "$1" >/dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "true"
    else
        echo "false"
    fi
}


# Update the system
sudo apt-get -y update
#sudo apt-get -y -o APT::Get::Always-Include-Phased-Updates=true upgrade
sudo apt-get -y upgrade

# https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 2.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 3.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 5.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 6.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 7.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0

export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
addLineToFile "export DOTNET_ROOT=$HOME/.dotnet" "$HOME/.bashrc"
addLineToFile "export PATH=\$PATH:\$DOTNET_ROOT:\$DOTNET_ROOT/tools" "$HOME/.bashrc"

dotnet tool install --global PowerShell 2>nul

result=$(doesCommandExist code)
if [ "$result" != "true" ]; then
    wget --no-clobber --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868 && sudo apt install -y ./code.deb && rm -f ./code.deb   
fi

export DONT_PROMPT_WSL_INSTALL=1 ;
addLineToFile "export DONT_PROMPT_WSL_INSTALL=1" "$HOME/.bashrc" 

mkdir -p "$HOME/source/repos"
mkdir -p "$HOME/source/packages"