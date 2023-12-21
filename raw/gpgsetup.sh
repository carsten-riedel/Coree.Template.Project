#!/usr/bin/env bash
# sudo curl -sSL https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/raw/gpgsetup.sh | bash

gpg_generate_key() {
    local user="$1"
    local mail="$2"
    local passphrase="$3"
    echo -e "Key-Type: RSA\nKey-Length: 4096\nName-Real: $user\nName-Comment: with stupid passphrase\nName-Email: $mail\nExpire-Date: 0\nPassphrase: $passphrase" | gpg --batch --generate-key -
}

gpg_get_key_id() {
    local user="$1"
    local key_id=$(gpg --list-signatures "$user" | grep '^sig' | awk '{print ($2 ~ /^[0-9]+$/ ? $3 : $2)}')
    echo "$key_id"
}

gpg_delete_user() {
    local user="$1"
    gpg --delete-secret-key $user
    gpg --delete-key $user
}

#gpg --delete-secret-key [uid]
#gpg --delete-key [uid]
