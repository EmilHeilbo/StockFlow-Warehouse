# StockFlow Warehouse

[![License: AGPL v3](https://img.shields.io/badge/License-AGPL_v3-blue.svg)](https://www.gnu.org/licenses/agpl-3.0)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-%23FE5196?logo=conventionalcommits&logoColor=white)](https://conventionalcommits.org)

This is a CRUD WebAPI for managing products and transactions in warehouses.

## Data Model

```mermaid
classDiagram
direction LR

	class Role {
	<<enumeration>>
	ADMIN
	WORKER
	READ_ONLY
	}
	
	class User {
	String APIkey
	Role Role
	String Username
	String PasswordHash
	String PasswordSalt
	}
	
	class Category {
	Guid Id
	String Name
	}
	
	class TransactionType {
	<<enumeration>>
	SALE
	PURCHASE
	RETURN
	MOVE
	}
	
	class Transaction {
	Guid Id
	TransactionType Type
	Dictionary &lt;Product product, int amount&gt; Products
	Recipient From
	Recipient To
	}
	
	class Product {
	Guid Id
	String Name
	List &lt;Category&gt; Cateories
	}
	
	class Recipient {
	Guid Id
	String Name
	String Adress
	}
	
	class Warehouse {
	Guid Id
	String Name
	String Address
	Dictionary &lt;Product product, int amount&gt; Products
	List &lt;Transaction&gt; Transactions
	}
	
	class Supplier {
	Guid Id
	String Name
	String Address
	}
	
	class Customer {
	Guid Id
	String Name
	String Address
	}
	
	User ..> Role
	Transaction ..> TransactionType
	IRecipient <|.. Warehouse : implements
	IRecipient <|.. Supplier : implements
	IRecipient <|.. Customer : implements
	Warehouse o-- Product
	Warehouse o-- Transaction
	Transaction o-- Recipient
	Product o-- Category
	```
