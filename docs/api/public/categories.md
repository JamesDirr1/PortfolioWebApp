[Docs](../index.md) / [API](../index.md) / [Categories](categories.md)

# Categories API

---

The Categories API provides public endpoints for retrieving active project categories, such as Comics, Painting, and
Photography.

> Public endpoint  
> Authentication is not required.

## Common Headers

| Header     | Required | Description                                                                            |
|------------|----------|----------------------------------------------------------------------------------------|
| Request-Id | No       | Optional GUID used for request tracing. If not provided, the server will generate one. |

## Request Tracing

Clients may include an `Request-Id` header to help trace requests. The value should ideally be a client-generated GUID.
This allows for easier correlation of requests and responses in logs and debugging tools.

If not provided, the server will generate a unique identifier and return it in the response headers.

---

# Endpoints

| Method | Endpoint             | Description                                       |
|--------|----------------------|---------------------------------------------------|
| GET    | /api/categories      | [GET all Categories](##GET`/api/categories`)      |
| GET    | /api/categories/{id} | [GET category by ID](##GET`/api/categories/{id}`) |

---

## GET `/api/categories`

### Description

Retrieves all active categories.

### Request

- **Method:** `GET`
- **URL:** `/api/categories`
- **Authentication:** Not required
- **Query Parameters:** None
- **Request Body:** None

### Response

#### Headers

- **Status Code:** `200 OK`
- **Content-Type:** application/json
- **Request-Id:** Echoes the `Request-Id` from the request or a generated one if not provided.

#### Body

Returns an array of category objects.

#### Response Fields

| Field          | Type    | Description                              |
|----------------|---------|------------------------------------------|
| `id`           | integer | Unique category identifier               |
| `title`        | string  | Display name of the category             |
| `slug`         | string  | URL-friendly category name               |
| `displayOrder` | integer | Controls display order                   |
| `description`  | string  | Short description of the category        |
| `isActive`     | boolean | Indicates whether the category is active |

#### Possible Status Codes

| Status Code               | Description                             |
|---------------------------|-----------------------------------------|
| 200 OK                    | Request was successful                  |
| 500 Internal Server Error | Something went wrong on the servers end |

### Example Requests

#### Successful

Example of a successful request

##### Request

```http 
GET /api/categories HTTP/1.1
Host: example.com
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

##### Response

```http 
HTTP/1.1 200 OK
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
[
  {
    "id": 1,
    "title": "Comics",
    "slug": "comics",
    "displayOrder": 1,
    "description": "Comic illustration and sequential art projects",
    "isActive": true
  },
  {
    "id": 2,
    "title": "Painting",
    "slug": "painting",
    "displayOrder": 2,
    "description": "Traditional and digital painting work",
    "isActive": true
  }
]
```

---

## GET `/api/categories/{id}`

### Description

Retrieves a specific category by its unique identifier.

### Request

- **Method:** `GET`
- **URL:** `/api/categories/{id: integer}`
- **Authentication:** Not required
- **Query Parameters:** {Parameters}
- **Request Body:** None

### Response

#### Headers

- **Status Code:** `200 OK`
- **Content-Type:** application/json
- **Request-Id:** Echoes the `Request-Id` from the request or a generated one if not provided.

#### Body

Returns a category object with the specified `id`.

If no category with the given `id` exists, a `404 Not Found` status will be returned.

#### Response Fields

| Field          | Type    | Description                              |
|----------------|---------|------------------------------------------|
| `id`           | integer | Unique category identifier               |
| `title`        | string  | Display name of the category             |
| `slug`         | string  | URL-friendly category name               |
| `displayOrder` | integer | Controls display order                   |
| `description`  | string  | Short description of the category        |
| `isActive`     | boolean | Indicates whether the category is active |

#### Possible Status Codes

| Status Code               | Description                                    |
|---------------------------|------------------------------------------------|
| 200 OK                    | Request was successful                         |
| 404 Not Found             | Indicates that request category does not exist |
| 500 Internal Server Error | Something went wrong on the servers end        |

### Example Requests

#### Successful

Example of a successful request

##### Request

```http 
GET /api/categories/1 HTTP/1.1
Host: example.com
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

##### Response

```http 
HTTP/1.1 200 OK
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "id": 1,
  "title": "Comics",
  "slug": "comics",
  "displayOrder": 1,
  "description": "Comic illustration and sequential art projects",
  "isActive": true
}
```

#### Not Found

Example of a request with an invalid id.

##### Request

```http 
GET /api/categories/-1 HTTP/1.1
Host: example.com
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

##### Response

```http 
HTTP/1.1 404 Not Found
Content-Type: application/json
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

```json
{
  "message": "Category not found"
}
```

---

- [Back to Endpoints](#endpoints)
- [Back to API Documentation](../index.md)
- [Back to Docs Home](../../index.md)