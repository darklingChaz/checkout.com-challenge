# checkout.com-challenge
Code base for the Checkout.com .NET Code Challenge 2.0


<br />
<br />

# ticket-1
- PaymentGateway app
- Auth
- Tests BBD / Unit
- Submit payments

<br />

# ticket-2
- Get Payment History

<br />

# ticket-3
- Added console bank simulation that asks user to verify each transaction

<br />

# bonus-docker
Containerized the Payment Gateway
- docker build -t payment-gateway:v1
- docker run -d --rm --name payment-gateway -p 3222:80 payment-gateway:v1
- curl "http://localhost:3222/info"


<br />
<br />
<br />
<br />

# Running the simulator
CAVEAT! - Please run the Similator in a Windows Console as it asks for user input.

To run the PaymentGateway and Simulator:
- Payment Gateway
    - cd ./app/PaymentGateway
    - dotnet run --launch-profile Production
    - http://localhost:5000/swagger
- Simulator
    - cd ./app/BankApiSimulator
    - dotnet run 

To use the swagger:
- Get an auth token using Username="User1", Password="Pwd1" (will last for 60mins)
- "Authorize" the other api endpoints by clicking "Authorize" (top right) and copying in token
- Try to submit, the BankApiSimulator should ask for verification (Y/N) for each transaction


# Moving forward
Ideas for improvement
- Better error details (ProblemDetails)
- Idempotency / Multiple retry issues
    - Make each auth token short lived and ensure that each transaction-per-token is unique and only succesfully made once.
    - More tests around locking of TransactionCache to help this
    - Would be interesting when trying to statelessly scale, maybe Redis with locks.
- HTTPS - Wanted to make this HTTPS only, but issues with the GitHub actions CI meant that I switched back the HTTP. Did not want to spend valuable interview/test time trying to fix Ubuntu/dotnet SSL cert issues
- AWS - I'd probably have it all behind an ApiGateway, let that deal with security, and proxy to the app