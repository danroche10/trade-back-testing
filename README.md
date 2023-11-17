# Crypto Trade Idea Backtester

## Project Overview

This project is designed to backtest cryptocurrency trading ideas using data from the CoinGecko API. Written in .NET, it allows users to apply their trading strategies to historical crypto market data to assess their potential effectiveness.

### Features

- **Data Retrieval**: Fetch historical cryptocurrency data via the CoinGecko API.
- **Strategy Testing**: Implement and test custom trading strategies.
- **Performance Analysis**: Evaluate the performance of strategies with various metrics.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download): Ensure you have the latest version of the .NET SDK installed on your machine.
- [Git](https://git-scm.com/downloads): Required for cloning the repository.

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/your-username/crypto-trade-backtester.git
   cd crypto-trade-backtester
   ```

2. **Install Dependencies**

   Navigate to the project directory and restore the dependencies:

   ```bash
   dotnet restore
   ```

### Running the Project Locally

1. **Compile the Project**

   Run the following command to build the project:

   ```bash
   dotnet build
   ```

2. **Run the Application**

   Once the build is successful, you can run the application:

   ```bash
   dotnet run
   ```

   This will start the application and begin backtesting based on the predefined strategies.
