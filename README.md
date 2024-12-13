# MiniWallet
Digital Wallet

 ## Wallet			*(=> endpoint => Create, Get details, update for FullName, update for PinCode )*
--------------
WalletUserId
WalletUserName
FullName			
MobileNo			
PinCode				=> (need to fill old pin and new pin) 
Balance

## Transaction		*(=> endpoint => create, get details)*
--------------
TransactionId
TransactionNo
TransactionDate
SenderMobileNo 
ReceiverMobileNo
Amount
Notes

## Deposit/Withdraw			=> endpoint ပေါ်မူတည်ပီး Deposit/Withdraw TransactionType မှာ update
----------------			*(=> endpoint => create, get details)*
Id
No
Date
Amount
MobileNo
TransactionType


### Validation

#### PinCode 
----------------------
=> old pin != new pin

#### Transaction
----------------------
=> From mobile, To mobile, balance, notes
=> Check From mobile exist
=> Check To mobile exist
=> From mobile != To mobile
=> Balance check
=> Notes check
=> PinCode check

Decrease (Debit) From MobileNo
Increase (Credit) To MobileNo
Create Transaction (insert)

#### Deposit/Withdraw
-----------------------
=> check mobile No
=> + amount

=> check mobile No
=> check balance
=> - amount
