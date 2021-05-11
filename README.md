# SecureFolder

### Commands

`-a --address ` Set the working directory.

`-d --decrypt ` Set crypto type to decrypt files.

`-e --encrypt ` Set crypto type to encrypt files.

`-f --file ` Input file to be processed.

`-p --password ` Set crypto cipher text.

`-r --remove ` Delete all origin files of given directory after encryption or decryption.

----------------------------------------------------

### To encrypt all files of a directory, execute below command in bash:

`> "YOUR_PATH\SecureFolder.exe" --encrypt --remove -address "YOUR_DIRECTORY_PATH\\DIR_NAME" `

or 

`> "YOUR_PATH\SecureFolder.exe" -e -r -a "YOUR_DIRECTORY_PATH\\DIR_NAME"`

### To decrypt all files of a directory, execute below command in bash:

`> "YOUR_PATH\SecureFolder.exe" --decrypt --remove -a "YOUR_DIRECTORY_PATH\\DIR_NAME" `

-----------------------------------------------------

![first page](https://github.com/bezzad/SecureFolder/raw/main/screenshot1.png)


### After enter password, your files encrypted to secure files.
![progress changing page](https://github.com/bezzad/SecureFolder/raw/main/screenshot2.png)
