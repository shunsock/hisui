#!/usr/bin/env bash
set -euo pipefail

# 引数チェック
if [ $# -ne 1 ]; then
    echo "Usage: $0 <ProjectName>"
    exit 1
fi

PROJECT_NAME="$1"

# ディレクトリ構成作成
mkdir -p "src/${PROJECT_NAME}" "tests/${PROJECT_NAME}.Tests"

# ソースプロジェクト作成
dotnet new console -n "${PROJECT_NAME}" -o "src/${PROJECT_NAME}"

# テストプロジェクト作成（NUnit）
dotnet new nunit -n "${PROJECT_NAME}.Tests" -o "tests/${PROJECT_NAME}.Tests"

# ソリューション作成
dotnet new sln -n "${PROJECT_NAME}"

# ソリューションに追加
dotnet sln "${PROJECT_NAME}.sln" add "src/${PROJECT_NAME}/${PROJECT_NAME}.csproj"
dotnet sln "${PROJECT_NAME}.sln" add "tests/${PROJECT_NAME}.Tests/${PROJECT_NAME}.Tests.csproj"

# テストプロジェクトからソースプロジェクトを参照
dotnet add "tests/${PROJECT_NAME}.Tests/${PROJECT_NAME}.Tests.csproj" reference "src/${PROJECT_NAME}/${PROJECT_NAME}.csproj"

# フォーマッターをインストール

echo "✅ ${PROJECT_NAME} skeleton created."
echo "📂 Structure:"
tree -L 3

