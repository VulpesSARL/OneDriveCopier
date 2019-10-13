# OneDrive copier

a small tool to copy files from local to OneDrive


### Usage examples

```
OneDriveCopier /?
```
Displays help text


```
OneDriveCopier /auth (guid)
```
Authenticate application with GUID to Microsoft (displays Login Window to complete the authentication)
Keys will be stored into the registry


```
OneDriveCopier /command UploadDir /uploadpath C:\FooBar /rpath documents/foobar
```
Copies files (and directories) from `C:\FooBar` to `documents/foobar` on OneDrive


### Note

All these tools are provided as-is from [Vulpes](https://vulpes.lu).
If you've some questions, contact me [here](https://go.vulpes.lu/contact).
