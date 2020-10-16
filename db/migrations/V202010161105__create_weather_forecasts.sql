CREATE TABLE weather_forecasts (
    id uuid PRIMARY KEY,
    city text NOT NULL,
    forecast_date timestamptz NOT NULL,
    forecast decimal(18, 4) NOT NULL
);