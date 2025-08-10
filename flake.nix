{
  description = "C# Shell";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-25.05";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };
      in {
        devShell = pkgs.mkShell {
          buildInputs = [
            pkgs.dotnet-sdk_9
            pkgs.go-task
            pkgs.tree
          ];
          DOTNET_ROOT = "${pkgs.dotnet-sdk_9}/share/dotnet";
          shellHook = ''
            echo "dotnet version: $(dotnet --version)"
            echo "task version: $(task --version)"
            echo "tree version: $(tree --version)"
            echo "DOTNET_ROOT: $DOTNET_ROOT"
          '';
        };
      });
}
