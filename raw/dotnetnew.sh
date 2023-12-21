#!/usr/bin/env bash
# sudo curl -sSL https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/raw/dotnetnew.sh | bash

createDirectoryIfNeeded() {
    local filepath="$1"
    local withsudo="$2"
    local dir="${filepath%/*}"

    # Check if the directory exists
    if [ ! -d "$dir" ]; then
        # Create the directory with sudo if withsudo is true
        if [ "$withsudo" = true ]; then
            sudo mkdir -p "$dir"
            echo "Directory created with sudo: $dir"
        else
            mkdir -p "$dir"
            echo "Directory created: $dir"
        fi
    else
        echo "Directory already exists: $dir"
    fi
}

addLineToFile() {
    local linetoadd="$1"
    local targetfile="$2"
    local withsudo="$3"

    if [ -z "$linetoadd" ]; then
        echo "No line provided to add."
        return 1
    fi

    if [ -z "$targetfile" ]; then
        echo "No target file specified."
        return 1
    fi

    if [ "$withsudo" != true ]; then
        if [ ! -w "$targetfile" ]; then
            echo "Target file is not writable without sudo."
        fi
    fi

    # Create directory if needed
    createDirectoryIfNeeded "$targetfile" "$withsudo"

    if [ ! -f "$targetfile" ] && [ "$withsudo" = true ]; then
            sudo touch "$targetfile"
        elif [ ! -f "$targetfile" ] && [ "$withsudo" != true ]; then
            touch "$targetfile"
        fi

    if ! grep -qF -- "$linetoadd" "$targetfile"; then

        if [ "$withsudo" = true ]; then
            echo "$linetoadd" | sudo tee -a "$targetfile" > /dev/null
        else
            echo "$linetoadd" | tee -a "$targetfile" > /dev/null
        fi

        echo "Line $linetoadd added to $targetfile"
    else
        echo "Line $linetoadd already present in $targetfile"
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

clear

sudo apt-get -y update
sudo apt-get -y upgrade

sudo apt-get -y install mc geany

curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 2.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 3.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 5.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 6.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 7.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0

dotnet tool install --global PowerShell 2>nul
dotnet tool install --global git-credential-manager 2>nul
