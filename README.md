# OFAC Search

A fancy dotnet webscraper for OFAC




## Documentation

[Documentation](https://learn.microsoft.com/en-us/dotnet/)


## Features

- Result dumping
- Single digit check
- Root folder ignoring


## Build

Build the OFAC-Search with dotnet

```bash
    cd OFAC_SEARCH

    dotnet build
    
    dotnet run

```
    
## Run compiled program

In the project structure - > bin -> *selected sdk core* -> ofac-search.exe


## Target folder structure
#### (single digit folders will be ignored and used for structuring)

```plaintext
root/
├─ a/
├─ b/
│  ├─ Bernard Jones/
├─ c/
│  ├─ Catherine Coraline/

```

