[Docs](../../index.md) / [API](../index.md) / [Categories](categories.md)

# Categories API

---

The Categories API provides public endpoints for retrieving active project categories, such as Comics, Painting, and
Photography.

> Public endpoint  
> Authentication is not required.

---

## Common Headers

| Header       | Required | Description                                                                            |
|--------------|----------|----------------------------------------------------------------------------------------|
| `Request-Id` | No       | Optional GUID used for request tracing. If not provided, the server will generate one. |

---

## Request Tracing

Clients may include an `Request-Id` header to help trace requests. The value should ideally be a client-generated GUID.
This allows for easier correlation of requests and responses in logs and debugging tools.

If not provided, the server will generate a unique identifier and return it in the response headers.

---

## Response Format

This endpoint uses the standard API response envelope.

```json
{
  "success": true,
  "message": "Request successful.",
  "data": {},
  "errors": []
}
```

---

## Models

### Category

| Field          | Type    | Description                              |
|----------------|---------|------------------------------------------|
| `id`           | integer | Unique category identifier               |
| `title`        | string  | Display name of the category             |
| `slug`         | string  | URL-friendly category name               |
| `displayOrder` | integer | Controls display order                   |
| `description`  | string  | Short description of the category        |
| `isActive`     | boolean | Indicates whether the category is active |

---

# Endpoints

| Method | Endpoint             | Description                                |
|--------|----------------------|--------------------------------------------|
| GET    | /api/categories      | [GET all Categories](#get-apicategories)   |
| GET    | /api/categories/{id} | [GET category by ID](#get-apicategoriesid) |

---

## GET `/api/categories`

### Description

Retrieves active categories with support for filtering, sorting, and pagination.

### Request

- **Method:** `GET`
- **URL:** `/api/categories`
- **Authentication:** Not required
- **Request Body:** None

#### Query Parameters

| Parameter       | Type    | Required | Default      | Description                                 |
|-----------------|---------|----------|--------------|---------------------------------------------|
| `title`         | string  | No       | null         | Filters categories by title.                |
| `sortBy`        | string  | No       | DisplayOrder | Sort field (`Id`, `Title`, `DisplayOrder`). |
| `sortDirection` | string  | No       | asc          | Sort direction (`asc` or `desc`).           |
| `page`          | integer | No       | 1            | Page number for pagination (must be >= 1).  |
| `pageSize`      | integer | No       | 10           | Number of items per page (1–100).           |

> `sortBy` and `sortDirection` are case-insensitive.

#### Example Requests

**HTTP**

```http request
GET /api/categories?sortBy=DisplayOrder&sortDirection=asc&page=1&pageSize=10 HTTP/1.1
Host: localhost:5018
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

**CURL**

```bash
curl -X GET "http://localhost:5018/api/categories?sortBy=DisplayOrder&sortDirection=asc&page=1&pageSize=10" \
  -H "Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49"
```

**REST METHOD**

```powershell
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Request-Id", "47ff3631-c08c-4a47-9dc3-32dd628d4a49")
$response = Invoke-RestMethod 'http://localhost:5018/api/categories?sortBy=DisplayOrder&sortDirection=asc&page=1&pageSize=10' -Method 'GET' -Headers $headers
$response | ConvertTo-Json
```

### Response

Returns a paginated list of [Category](#category) objects.

The response is wrapped in the standard API response envelope.

#### Headers

- **Status Code:** `200 OK`
- **Content-Type:** application/json
- **Request-Id:** Echoes the `Request-Id` from the request or a generated one if not provided.

#### Pagination Structure

The `data` field contains a paginated response:

| Field      | Type   | Description              |
|------------|--------|--------------------------|
| `items`    | array  | List of Category objects |
| `metaData` | object | Pagination metadata      |

#### Possible Status Codes

| Status Code               | Description                             |
|---------------------------|-----------------------------------------|
| 200 OK                    | Request was successful                  |
| 400 Bad Request           | Indicates invalid query parameters      |
| 500 Internal Server Error | Something went wrong on the servers end |

#### Example Responses

##### Successful `200 OK`

```http request
HTTP/1.1 200 OK
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": true,
  "message": "Categories retrieved successfully.",
  "data": {
    "items": [
      {
        "id": 1,
        "title": "Comics",
        "slug": "comics",
        "displayOrder": 1,
        "description": "Comic illustration and sequential art projects",
        "isActive": true
      }
    ],
    "metaData": {
      "page": 1,
      "pageSize": 10,
      "totalCount": 1,
      "totalPages": 1,
      "hasPreviousPage": false,
      "hasNextPage": false
    }
  },
  "errors": []
}
```

##### Empty Result `200 OK`

```http request
HTTP/1.1 200 OK
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": true,
  "message": "No categories found.",
  "data": {
    "items": [],
    "metaData": {
      "page": 1,
      "pageSize": 10,
      "totalCount": 0,
      "totalPages": 0,
      "hasPreviousPage": false,
      "hasNextPage": false
    }
  },
  "errors": []
}
```

##### Validation Error `400 Bad Request`

```http request
HTTP/1.1 400 Bad Request
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": false,
  "message": "Validation failed.",
  "data": null,
  "errors": {
    "SortBy": [
      "SortBy must be one of: Id, Title, DisplayOrder."
    ]
  }
}
```

---

## GET `/api/categories/{id}`

### Description

Retrieves a specific category by its unique identifier.

### Request

- **Method:** `GET`
- **URL:** `/api/categories/{id}`
- **Authentication:** Not required
- **Path Parameters:** `{id: integer}`
- **Request Body:** None

#### Path Parameters

| Parameter | Type    | Required | Description                                  |
|-----------|---------|----------|----------------------------------------------|
| `id`      | integer | Yes      | Unique identifier of the requested category. |

### Example Requests

#### HTTP

```http request
GET /api/categories/1 HTTP/1.1
Host: localhost:5018
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

#### CURL

```curl
curl -X GET "http://localhost:5018/api/categories/1" \
  -H "Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49"
```

#### REST METHOD

```powershell
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Request-Id", "47ff3631-c08c-4a47-9dc3-32dd628d4a49")
$response = Invoke-RestMethod 'http://localhost:5018/api/categories/1' -Method 'GET' -Headers $headers
$response | ConvertTo-Json
```

### Response

Returns a single [Category](#category) object.

The response is wrapped in the standard API response envelope.

#### Headers

- **Status Code:** `200 OK`
- **Content-Type:** application/json
- **Request-Id:** Echoes the `Request-Id` from the request or a generated one if not provided.

#### Possible Status Codes

| Status Code               | Description                                          |
|---------------------------|------------------------------------------------------|
| 200 OK                    | Request was successful                               |
| 400 Bad Request           | Indicates an invalid category id                     |
| 404 Not Found             | Indicates that the requested category does not exist |
| 500 Internal Server Error | Something went wrong on the server                   |

### Example Responses

#### Successful `200 OK`

```http 
HTTP/1.1 200 OK
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": true,
  "message": "Category retrieved successfully.",
  "data": {
    "id": 1,
    "title": "Comics",
    "slug": "comics",
    "displayOrder": 1,
    "description": "Comic illustration and sequential art projects",
    "isActive": true
  },
  "errors": []
}
```

#### Invalid ID `400 Bad Request`

```http 
HTTP/1.1 400 Bad Request
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": false,
  "message": "Invalid category id.",
  "data": null,
  "errors": [
    "'abc' is not a valid category id. Id must be an integer greater than 0."
  ]
}
```

#### Invalid ID `404 Not Found`

Example of a request for a category that does not exist.

##### Request

```http 
HTTP/1.1 404 Not Found
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "success": false,
  "message": "Category not found.",
  "data": null,
  "errors": [
    "No category exists with id 999."
  ]
}
```

---

- [Back to Endpoints](#endpoints)
- [Back to API Documentation](../index.md)
- [Back to Docs Home](../../index.md)