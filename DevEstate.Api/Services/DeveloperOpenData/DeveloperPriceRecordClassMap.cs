using CsvHelper.Configuration;
using DevEstate.Dtos.DeveloperOpenData;

public class DeveloperPriceRecordClassMap : ClassMap<DeveloperPriceRecord>
{
    public DeveloperPriceRecordClassMap()
    {
        // -------------------------------
        // LOKALIZACJE (nigdy nie crashują)
        // -------------------------------

        Map(m => m.Wojewodztwo)
            .Optional()
            .Name(
                "województwo lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - województwo",
                "województwo",
                "region"
            )
            .Default("")
            .TypeConverterOption.NullValues("x", "-", "", "null", "n/a");

        Map(m => m.Powiat)
            .Optional()
            .Name(
                "powiat lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - powiat",
                "powiat"
            )
            .Default("")
            .TypeConverterOption.NullValues("x", "-", "", "null", "n/a");

        Map(m => m.Gmina)
            .Optional()
            .Name(
                "gmina lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - gmina",
                "gmina"
            )
            .Default("")
            .TypeConverterOption.NullValues("x", "-", "", "null", "n/a");

        Map(m => m.Miejscowosc)
            .Optional()
            .Name(
                "miejscowość lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - miejscowość",
                "miejscowość",
                "miejscowosc"
            )
            .Default("")
            .TypeConverterOption.NullValues("x", "-", "", "null", "n/a");

        // -------------------------------
        // CENA ZA M2 (odporna na cokolwiek)
        // -------------------------------

        Map(m => m.CenaZaM2)
            .Optional()  // jeśli brak kolumny → null
            .Name(
                "cena za m2 nieruchomości",
                "cena m2",
                "cena m 2",
                "cena m 2 powierzchni użytkowej lokalu mieszkalnego / domu jednorodzinnego [zł]"
            )
            .TypeConverter<DecimalNullableConverter>() // nasz tolerant converter
            .Default((decimal?)null);
    }
}

