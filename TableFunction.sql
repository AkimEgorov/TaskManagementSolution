-- =============================================
-- 1. Тестовая таблица и данные
-- =============================================
DROP TABLE IF EXISTS ClientPayments;

CREATE TABLE ClientPayments (
    Id BIGINT PRIMARY KEY,
    ClientId BIGINT,
    Dt TIMESTAMP,
    Amount MONEY
);

INSERT INTO ClientPayments (Id, ClientId, Dt, Amount) VALUES
(1, 1, '2022-01-03 17:24:00', 100),
(2, 1, '2022-01-05 17:24:14', 200),
(3, 1, '2022-01-05 18:23:34', 250),
(4, 1, '2022-01-07 10:12:38', 50),
(5, 2, '2022-01-05 17:24:14', 278),
(6, 2, '2022-01-10 12:39:29', 300);

-- =============================================
-- 2. Табличная функция
-- =============================================
CREATE OR REPLACE FUNCTION fn_get_client_daily_payments(
    p_client_id BIGINT,
    p_sd DATE,
    p_ed DATE
)
RETURNS TABLE (
    Dt DATE,
    Сумма MONEY
)
LANGUAGE sql
AS $$
    WITH DateSeries AS (
        SELECT generate_series(p_sd, p_ed, INTERVAL '1 day')::DATE AS dt
    ),
    PaymentSums AS (
        -- Агрегируем платежи только для нужного клиента и интервала
        SELECT 
            Dt::DATE AS payment_date,
            SUM(Amount) AS total_amount
        FROM ClientPayments
        WHERE ClientId = p_client_id
          AND Dt::DATE >= p_sd
          AND Dt::DATE <= p_ed
        GROUP BY Dt::DATE
    )
    -- Соединяем полный ряд дат с агрегированными суммами
    SELECT 
        ds.dt AS Dt,
        COALESCE(ps.total_amount, 0::MONEY) AS Сумма
    FROM DateSeries ds
    LEFT JOIN PaymentSums ps ON ds.dt = ps.payment_date
    ORDER BY ds.dt;
$$;

-- =============================================
-- 3. Проверка на примерах из ТЗ
-- =============================================

-- Пример 1: ClientId = 1, Sd = '2022-01-02', Ed = '2022-01-07'
SELECT * FROM fn_get_client_daily_payments(1, '2022-01-02', '2022-01-07');

-- Пример 2: ClientId = 2, Sd = '2022-01-04', Ed = '2022-01-11'
SELECT * FROM fn_get_client_daily_payments(2, '2022-01-04', '2022-01-11');