# CoinMonitorService
Coin Monitor windows service for data fetching, storage, handling.

This was made as a service for crypto coin price monitoring. The generic implementation supports any exchange rest api, currently two of them are implementet (cex.io and poloniex).
This service gets the current price data, formats it to a generic style and calculates eco index values.
A more generic coin index calculation implementation is needed (coming soon)
And a library for Elastic support is needed (currently everything is made by hand via REST request) (this will be reworked soon)
