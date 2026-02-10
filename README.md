![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Web API](https://img.shields.io/badge/Web-API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![Version](https://img.shields.io/badge/API%20Version-v1-blue?style=for-the-badge)


## Code Bars Generator

O Code Bars Generator é uma API que tem a função de gerar código de barras e QR Code que podem ser utilizados em etiquetas ou relatórios.
Ele foi desenvolvido para suprir a demanda de geração de etiquetas utilizadas no Crystal Reports, empregado no SAP Business One (SAP B1), que possui limitações no consumo de serviços externos via HTTPS.

## API Endpoints
```text
  - GET /api/v1/GenCodeBars/{code} 
  → Retorna um código de barra no formato BMP

  - GET /api/v1/GenQrCodes/{code} 
  → Retorna um QR Code no formato BMP
```
## Exemplo de Uso

#### Gerar Código de Barras

- GET /api/v1/GenCodeBars/123456789

  <p align="center">
  <img src="https://github.com/user-attachments/assets/6af8af1a-c801-4feb-b846-74d68f206cae" width="300" />
  <br/>
  <em>Barcode image generated in BMP format</em>
</p>

#### Gerar QR Code
- GET /api/v1/GenQrCodes/123456789

  <p align="center">
  <img src="https://github.com/user-attachments/assets/f6dc2331-6e18-4d13-b821-89405405344c" width="300" />
  <br/>
  <em>QR Code image generated in BMP format</em>
</p>

| Pacote | Versão | 
| :--- | :--- |
| ZXing.Net | 0.16.11 | 
| Sentry.AspNet Core | 6.1.0 | 
| ZXing.Net.Bindings.Windows.Compatibility | 0.16.14 |
| SixLabors.ImageSharp | 3.1.12 |



## EN - ENGLISH Code Bars Generator

Code Bars Generator is an API designed to generate barcodes and QR codes that can be used in labels and reports.
It was developed to meet the demand for label generation in Crystal Reports, used within SAP Business One (SAP B1), which has limitations when consuming external HTTPS services.

### API Endpoints

- **GET** `/api/v1/GenCodeBars/{code}`  
  Returns a barcode image in **BMP** format.

- **GET** `/api/v1/GenQrCodes/{code}`  
  Returns a QR code image in **BMP** format.
