# Psxbox.Utils

**Psxbox.Utils** - umumiy yordamchi funksiyalar, konverterlar va calculatorlarni o'z ichiga olgan kutubxona.

## Xususiyatlari

- ?? **Converters** - Hex, Binary, Encoding konvertatsiyalari
- ?? **Calculators** - Matematik va statistik hisoblashlar
- ?? **DateTime Helpers** - Sana va vaqt bilan ishlash uchun utilitar
- ?? **Value Utils** - Ma'lumot turlarini boshqarish
- ?? **Array Helpers** - Massivlar bilan ishlash
- ?? **String Helpers** - String operatsiyalari
- ?? **Boolean Helpers** - Boolean qiymatlarni boshqarish

## O'rnatish

```bash
dotnet add reference Shared/Psxbox.Utils/Psxbox.Utils.csproj
```

## Bog'liqliklar

Hech qanday tashqi paket talab qilinmaydi (to'liq standalone kutubxona).

## Foydalanish

### Converters - Konvertatsiyalar

```csharp
using Psxbox.Utils;

// Hex string ni byte array ga
byte[] data = Converters.HexStringToByteArray("A1B2C3D4");

// Byte array ni hex string ga
string hex = Converters.ByteArrayToHexString(data);

// CP866 encoding (Kirill belgilar uchun)
string text = Converters.DecodeCP866(byteData);

// BCD (Binary-Coded Decimal) konvertatsiya
byte[] bcdData = Converters.NumbersToBCD("12345678");
string number = Converters.BCDToString(bcdData);

// Endianness boshqaruvi
ushort value = Converters.SwapBytes(0x1234); // 0x3412 qaytaradi
```

### Calculators - Matematik Hisoblashlar

```csharp
using Psxbox.Utils;

// Statistik hisoblashlar
double[] values = { 1.5, 2.3, 3.1, 4.8, 5.2 };
double average = Calculators.Average(values);
double min = Calculators.Min(values);
double max = Calculators.Max(values);
double sum = Calculators.Sum(values);

// Quvvat faktori hisoblash
double pf = Calculators.CalculatePowerFactor(activePower, reactivePower);

// CRC hisoblash
ushort crc16 = Calculators.CalculateCRC16(data);
byte crc8 = Calculators.CalculateCRC8(data);
```

### DateTimeHelpers - Sana va Vaqt

```csharp
using Psxbox.Utils.Helpers;

// Unix timestamp konvertatsiyasi
long unixTime = DateTimeHelpers.ToUnixTimestamp(DateTime.UtcNow);
DateTime dateTime = DateTimeHelpers.FromUnixTimestamp(unixTime);

// Davr hisoblash
Period period = new Period
{
    Start = DateTime.Today.AddDays(-7),
    End = DateTime.Today
};

int days = period.Days;
int hours = period.Hours;

// TimeZone bilan ishlash
DateTimeWithZone dtz = new DateTimeWithZone(DateTime.UtcNow, TimeZoneInfo.Local);
DateTimeOffset localTime = dtz.LocalTime;
```

### ArrayHelpers - Massiv Operatsiyalari

```csharp
using Psxbox.Utils.Helpers;

// Massivlarni birlashtirish
byte[] combined = ArrayHelpers.Concat(array1, array2, array3);

// Qism massiv olish
byte[] slice = ArrayHelpers.Slice(data, startIndex: 5, length: 10);

// Massiv aylanish (rotate)
byte[] rotated = ArrayHelpers.Rotate(data, positions: 3);

// Massivlarni solishtirish
bool isEqual = ArrayHelpers.AreEqual(array1, array2);
```

### StringHelpers - String Operatsiyalari

```csharp
using Psxbox.Utils.Helpers;

// String tozalash
string cleaned = StringHelpers.RemoveSpecialCharacters("Hello@World#123");

// Qidiruv
bool contains = StringHelpers.ContainsIgnoreCase(text, "search");

// Truncate
string truncated = StringHelpers.Truncate(longText, maxLength: 50, suffix: "...");

// Padding
string padded = StringHelpers.PadBoth(text, totalWidth: 20);
```

### BooleanHelpers - Boolean Operatsiyalari

```csharp
using Psxbox.Utils.Helpers;

// String dan boolean
bool value1 = BooleanHelpers.ParseBoolean("yes");  // true
bool value2 = BooleanHelpers.ParseBoolean("1");    // true
bool value3 = BooleanHelpers.ParseBoolean("on");   // true
bool value4 = BooleanHelpers.ParseBoolean("no");   // false

// Boolean ni string ga
string text = BooleanHelpers.ToString(true, format: "Yes/No");
```

### ValueUtils - Qiymat Operatsiyalari

```csharp
using Psxbox.Utils;

// Null tekshirish va default qiymat
int value = ValueUtils.GetValueOrDefault(nullableInt, defaultValue: 0);

// Type konvertatsiya
object result = ValueUtils.ConvertTo<double>("123.45");

// Range check
bool inRange = ValueUtils.IsInRange(value, min: 0, max: 100);
```

## Ishlatiladigan Joylar

Bu kutubxona MyGateway tizimida quyidagi joylarda ishlatiladi:

- ?? **Protokol implementatsiyalari** - Modbus, ROC, Mercury, CE30X, DLMS
- ?? **Ma'lumot konvertatsiyasi** - Hex, BCD, Encoding
- ?? **CRC hisoblash** - Modbus CRC16, LRC8
- ?? **Vaqt bilan ishlash** - Timezone, Unix timestamp
- ?? **Matematik operatsiyalar** - Power factor, averages

## Encoding Qo'llab-quvvatlash

Quyidagi encoding'lar qo'llab-quvvatlanadi:
- UTF-8
- UTF-16
- ASCII
- CP866 (Kirill belgilar)
- Windows-1251
- KOI8-R

## Litsenziya

MIT License

## Muallif

Psxbox (adhamyu83@gmail.com)
