# checkout.com-challenge
Code base for the Checkout.com .NET Code Challange 2.0


<br />
<br />

# ticket-1
First implementation.  
- PaymentGateway app
- Auth
- Tests BBD / Unit
- Submit payments

<br />

# ticket-2
Second implementation.
- Get Payment History


<br />
<br />

# NOTES
- Wanted to make this HTTPS only, but issues with the GitHub actions CI meant that I switched back the HTTP. Did not want to spend valuable interview/test time trying to fix Ubuntu/dotnet SSL cert issues.
-- Also, run it in a container, properly configured and don't open any other port, that helps :)
