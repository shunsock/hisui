# Clean Architecture 設計書

## 1. 背景と目的

### 現状

hisui は全角・半角変換を行う CLI ツールである。現在のコード構成は以下の通り:

```
src/hisui/
├── Program.cs                  # Cocona CLI エントリポイント（直接 TextWidthConverter を呼び出す）
└── TextWidth/
    └── TextWidthConverter.cs   # 静的ユーティリティクラス
```

Program.cs がドメインロジック（TextWidthConverter）を直接参照しており、レイヤー間の境界が存在しない。

### 目的

- ドメインロジックをフレームワーク（Cocona）から分離する
- 依存性逆転により、ドメイン層が外部に依存しない構造にする
- CQRS パターンで外部アクセスの意図を明確にする
- 将来的な機能追加（新しい変換コマンド、外部辞書参照など）に対して開放的な構造にする

## 2. レイヤー構成

```
src/hisui/
├── Domain/
│   ├── ValueObject/
│   │   ├── UnicodeText.cs
│   │   └── ConversionDirection.cs
│   └── DomainService/
│       └── TextWidthConversionService.cs
├── UseCase/
│   ├── Query/
│   │   ├── IConvertTextWidthQuery.cs
│   │   └── ConvertTextWidthQuery.cs
│   └── Handler/
│       ├── IConvertTextWidthHandler.cs
│       └── ConvertTextWidthHandler.cs
├── Infrastructure/
│   └── Presenter/
│       ├── IOutputPort.cs
│       └── ConsoleOutputAdapter.cs
└── Program.cs
```

依存方向: `Infrastructure → UseCase → Domain`（外から内への一方向）

## 3. Domain 層

### 3.1 Value Object

#### UnicodeText

変換対象のテキストを表現する値オブジェクト。空文字列は許可するが null は拒否する。

```csharp
namespace Hisui.Domain.ValueObject;

/// <summary>
/// 変換対象のUnicodeテキストを表現する値オブジェクト。
/// null を排除し、ドメイン内で安全に扱えることを保証する。
/// </summary>
public sealed class UnicodeText : IEquatable<UnicodeText>
{
    public string Value { get; }

    public UnicodeText(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public bool Equals(UnicodeText? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as UnicodeText);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
```

**設計判断:**
- `string` をそのまま使わず型で区別することで、引数の取り違えを型レベルで防止する
- 不変オブジェクトとして設計し、生成後の状態変更を禁止する
- null チェックをコンストラクタに集約し、ドメイン内部では null 安全を前提にできる

#### ConversionDirection

変換方向を表す列挙型。

```csharp
namespace Hisui.Domain.ValueObject;

/// <summary>
/// テキスト幅の変換方向を示す。
/// </summary>
public enum ConversionDirection
{
    /// <summary>全角 → 半角</summary>
    ToHalfWidth,

    /// <summary>半角 → 全角</summary>
    ToFullWidth,
}
```

**設計判断:**
- コマンド名（`f2h` / `h2f`）という表示層の概念をドメインに持ち込まない
- 列挙型で有限の選択肢を型で制約する

### 3.2 Domain Service

#### TextWidthConversionService

全角・半角変換のドメインロジックを担う。現在の `TextWidthConverter` のロジックをここに移動する。

```csharp
namespace Hisui.Domain.DomainService;

using Hisui.Domain.ValueObject;

/// <summary>
/// Unicode テキストの全角・半角変換を行うドメインサービス。
/// </summary>
public static class TextWidthConversionService
{
    /// <summary>
    /// 指定された方向にテキスト幅を変換する。
    /// </summary>
    public static UnicodeText Convert(UnicodeText input, ConversionDirection direction)
    {
        var converted = direction switch
        {
            ConversionDirection.ToHalfWidth => ToHalfWidth(input.Value),
            ConversionDirection.ToFullWidth => ToFullWidth(input.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
        return new UnicodeText(converted);
    }

    // ToHalfWidth, ToFullWidth の実装は現在の TextWidthConverter から移植
    private static string ToHalfWidth(string input) { /* 既存ロジック */ }
    private static string ToFullWidth(string input) { /* 既存ロジック */ }
}
```

**設計判断:**
- 変換ロジックは特定のエンティティに属さない横断的な処理であるため、Domain Service とする
- `Convert` メソッドで方向を受け取ることで、呼び出し側の分岐を不要にする
- 入出力を `UnicodeText` に統一し、生の `string` がドメイン境界を越えないようにする

### 3.3 Entity について

現在の hisui は状態を持たないテキスト変換ツールであり、ライフサイクルやIDで識別されるドメインオブジェクトが存在しない。そのため Entity 層は**設けない**。

将来的に変換履歴の管理や辞書エンティティが必要になった場合に `Domain/Entity/` を追加する。

## 4. UseCase 層（CQRS）

テキスト変換は入力を受け取り結果を返す読み取り操作であるため、Query として設計する。

### 4.1 Query

#### IConvertTextWidthQuery

Query の契約を定義するインターフェース。

```csharp
namespace Hisui.UseCase.Query;

using Hisui.Domain.ValueObject;

/// <summary>
/// テキスト幅変換クエリの契約。
/// </summary>
public interface IConvertTextWidthQuery
{
    UnicodeText Input { get; }
    ConversionDirection Direction { get; }
}
```

#### ConvertTextWidthQuery

Query の実装。不変のデータ転送オブジェクト。

```csharp
namespace Hisui.UseCase.Query;

using Hisui.Domain.ValueObject;

/// <summary>
/// テキスト幅変換のクエリ。入力テキストと変換方向を保持する。
/// </summary>
public sealed class ConvertTextWidthQuery : IConvertTextWidthQuery
{
    public UnicodeText Input { get; }
    public ConversionDirection Direction { get; }

    public ConvertTextWidthQuery(UnicodeText input, ConversionDirection direction)
    {
        Input = input;
        Direction = direction;
    }
}
```

### 4.2 Handler

#### IConvertTextWidthHandler

Handler の契約。UseCase 層で定義し、Infrastructure からの依存性逆転を可能にする。

```csharp
namespace Hisui.UseCase.Handler;

using Hisui.Domain.ValueObject;
using Hisui.UseCase.Query;

/// <summary>
/// テキスト幅変換クエリを処理するハンドラの契約。
/// </summary>
public interface IConvertTextWidthHandler
{
    UnicodeText Handle(IConvertTextWidthQuery query);
}
```

#### ConvertTextWidthHandler

Handler の実装。Domain Service を呼び出して変換を実行する。

```csharp
namespace Hisui.UseCase.Handler;

using Hisui.Domain.DomainService;
using Hisui.Domain.ValueObject;
using Hisui.UseCase.Query;

/// <summary>
/// テキスト幅変換クエリのハンドラ実装。
/// </summary>
public sealed class ConvertTextWidthHandler : IConvertTextWidthHandler
{
    public UnicodeText Handle(IConvertTextWidthQuery query)
    {
        return TextWidthConversionService.Convert(query.Input, query.Direction);
    }
}
```

**設計判断:**
- 現時点では Handler が薄いラッパーに見えるが、将来的にログ記録・バリデーション・キャッシュなどの横断的関心事をここに挿入できる
- Query/Handler を分離することで、テスト時に Handler を差し替え可能にする
- CQRS の Query 側のみ実装する。Command（副作用を伴う操作）は現時点で不要

## 5. Infrastructure 層

### 5.1 Presenter

#### IOutputPort

出力先の抽象化。UseCase 層に定義してもよいが、現在は Infrastructure 層でのみ使用するため Infrastructure に配置する。

```csharp
namespace Hisui.Infrastructure.Presenter;

using Hisui.Domain.ValueObject;

/// <summary>
/// 変換結果の出力先を抽象化するポート。
/// </summary>
public interface IOutputPort
{
    void Present(UnicodeText result);
}
```

#### ConsoleOutputAdapter

標準出力への書き込みを担うアダプタ。

```csharp
namespace Hisui.Infrastructure.Presenter;

using Hisui.Domain.ValueObject;

/// <summary>
/// 変換結果をコンソール（標準出力）に出力するアダプタ。
/// </summary>
public sealed class ConsoleOutputAdapter : IOutputPort
{
    public void Present(UnicodeText result)
    {
        Console.WriteLine(result.Value);
    }
}
```

### 5.2 Program.cs（Composition Root）

DI コンテナの構成と Cocona コマンドの登録を行うエントリポイント。

```csharp
using Cocona;
using Hisui.Domain.ValueObject;
using Hisui.Infrastructure.Presenter;
using Hisui.UseCase.Handler;
using Hisui.UseCase.Query;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IConvertTextWidthHandler, ConvertTextWidthHandler>();
builder.Services.AddSingleton<IOutputPort, ConsoleOutputAdapter>();

var app = builder.Build();

app.AddCommand("f2h", (string src, IConvertTextWidthHandler handler, IOutputPort output) =>
{
    var query = new ConvertTextWidthQuery(
        new UnicodeText(src),
        ConversionDirection.ToHalfWidth
    );
    var result = handler.Handle(query);
    output.Present(result);
});

app.AddCommand("h2f", (string src, IConvertTextWidthHandler handler, IOutputPort output) =>
{
    var query = new ConvertTextWidthQuery(
        new UnicodeText(src),
        ConversionDirection.ToFullWidth
    );
    var result = handler.Handle(query);
    output.Present(result);
});

app.Run();
```

**設計判断:**
- Cocona の DI 機能を活用し、Composition Root で依存関係を組み立てる
- コマンドハンドラ内では Value Object への変換 → Query 生成 → Handler 呼び出し → 出力の流れを明示する
- Program.cs はオーケストレーションのみ担い、ビジネスロジックを持たない

## 6. テスト戦略

### 6.1 レイヤー別テスト方針

| レイヤー | テスト種別 | 方針 |
|---------|-----------|------|
| Domain/ValueObject | 単体テスト | 生成制約・等値性の検証 |
| Domain/DomainService | 単体テスト | 既存テストを移植。変換ロジックの網羅的検証 |
| UseCase/Handler | 単体テスト | Handler が Domain Service を正しく呼び出すことの検証 |
| Infrastructure/Presenter | 統合テスト | コンソール出力の検証（StringWriter で差し替え） |
| Program.cs (E2E) | 統合テスト | CLI コマンドの入出力検証 |

### 6.2 テストディレクトリ構成

```
tests/hisui.Tests/
├── Domain/
│   ├── ValueObject/
│   │   ├── UnicodeTextTests.cs
│   │   └── ConversionDirectionTests.cs
│   └── DomainService/
│       └── TextWidthConversionServiceTests.cs    # 既存テストから移植
├── UseCase/
│   └── Handler/
│       └── ConvertTextWidthHandlerTests.cs
└── Infrastructure/
    └── Presenter/
        └── ConsoleOutputAdapterTests.cs
```

## 7. 依存グラフ

```
Program.cs (Composition Root)
    │
    ├── Infrastructure.Presenter
    │       └── IOutputPort ← ConsoleOutputAdapter
    │
    ├── UseCase.Handler
    │       └── IConvertTextWidthHandler ← ConvertTextWidthHandler
    │               │
    │               └── UseCase.Query
    │                       └── IConvertTextWidthQuery ← ConvertTextWidthQuery
    │
    └── Domain
            ├── ValueObject
            │       ├── UnicodeText
            │       └── ConversionDirection
            └── DomainService
                    └── TextWidthConversionService
```

- Domain は他のどの層にも依存しない
- UseCase は Domain のみに依存する
- Infrastructure は UseCase と Domain に依存する
- Program.cs（Composition Root）は全層を参照し、DI で結合する

## 8. 名前空間の対応

| 変更前 | 変更後 |
|--------|--------|
| `hisui.TextWidth.TextWidthConverter` | `Hisui.Domain.DomainService.TextWidthConversionService` |
| — | `Hisui.Domain.ValueObject.UnicodeText` |
| — | `Hisui.Domain.ValueObject.ConversionDirection` |
| — | `Hisui.UseCase.Query.IConvertTextWidthQuery` |
| — | `Hisui.UseCase.Query.ConvertTextWidthQuery` |
| — | `Hisui.UseCase.Handler.IConvertTextWidthHandler` |
| — | `Hisui.UseCase.Handler.ConvertTextWidthHandler` |
| — | `Hisui.Infrastructure.Presenter.IOutputPort` |
| — | `Hisui.Infrastructure.Presenter.ConsoleOutputAdapter` |

## 9. 移行手順

1. **ディレクトリ構造の作成** — Domain, UseCase, Infrastructure の各ディレクトリを作成
2. **Value Object の実装** — UnicodeText, ConversionDirection を作成しテストを書く
3. **Domain Service の実装** — TextWidthConversionService を作成し、既存テストを移植
4. **UseCase の実装** — Query, Handler を作成しテストを書く
5. **Infrastructure の実装** — IOutputPort, ConsoleOutputAdapter を作成
6. **Program.cs の書き換え** — Composition Root として再構成
7. **旧コードの削除** — TextWidth/ ディレクトリを削除
8. **全テスト実行** — 既存の振る舞いが保持されていることを確認
