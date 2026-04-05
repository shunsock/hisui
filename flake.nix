{
  description = "Hisui - Text width conversion CLI tool";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-25.05";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };
        dotnet-sdk = pkgs.dotnetCorePackages.sdk_10_0;
        dotnet-runtime = pkgs.dotnetCorePackages.runtime_10_0;
      in {
        packages.default = pkgs.buildDotnetModule {
          pname = "hisui";
          version = "0.1.0";
          src = ./.;

          projectFile = "src/hisui/hisui.csproj";
          nugetDeps = ./deps.json;

          inherit dotnet-sdk dotnet-runtime;

          executables = [ "hisui" ];
        };

        devShells.default = pkgs.mkShell {
          buildInputs = [
            dotnet-sdk
            pkgs.go-task
            pkgs.tree
          ];
          DOTNET_ROOT = "${dotnet-sdk}/share/dotnet";
          shellHook = ''
            echo "dotnet version: $(dotnet --version)"
            echo "task version: $(task --version)"
            echo "tree version: $(tree --version)"
            echo "DOTNET_ROOT: $DOTNET_ROOT"
          '';
        };
      });
}
