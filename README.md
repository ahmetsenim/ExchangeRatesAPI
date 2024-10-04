# .NET Core 8.0 TCMB Döviz Kurları Apisi

Bu proje, Türk Merkez Bankası'ndan (TCMB) döviz kurlarını çeken ve kullanıcılara sunan bir .NET Core 8 web API'sidir. API, döviz kurlarını 10 dakika boyunca bellekte cache'ler ve TCMB API'sine ulaşılamadığı durumda verileri yerel bir yedek dosyadan çeker, böylece hizmet kesintisiz bir şekilde sunulmaya devam eder. GetExchangeRatesAsync methoduna gönderilen 'currenyCode' parametresi ile istenen para birimine dönüşüm işlemleri yapılarak istemciye güncel döviz kuru bilgileri döndürülür.

## Özellikler

- **Döviz Kurları**: API, en güncel döviz kurlarını [TCMB](https://www.tcmb.gov.tr/) sitesinden çeker.
- **Caching**: Döviz kurları, performansı optimize etmek için 10 dakika boyunca bellekte saklanır.
- **Local Backup**: TCMB API'sine ulaşılamadığında, API verileri yerel yedek dosyasından alır.
- **Clean Code**: Proje, bağımlılıkların yönetimi (Dependency Injection) ve sorumluluk ayrımı (Separation of Concerns) prensiplerine göre yapılandırılmıştır.
