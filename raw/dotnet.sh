#!/usr/bin/env bash
# sudo curl -sSL https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/raw/dotnet.sh | bash
clear
sudo apt-get update && sudo apt-get -y upgrade
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 6.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 7.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0
export DOTNET_ROOT=$HOME/.dotnet ; export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools ; echo 'export DOTNET_ROOT=$HOME/.dotnet' >> $HOME/.bashrc && echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> $HOME/.bashrc
dotnet tool install --global PowerShell
sudo wget --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868 && sudo apt install -y ./code.deb && rm -f ./code.deb
export DONT_PROMPT_WSL_INSTALL=1 ; echo 'export DONT_PROMPT_WSL_INSTALL=1' >> $HOME/.bashrc ; mkdir -p "$HOME/source/repos" ; mkdir -p "$HOME/source/packages"