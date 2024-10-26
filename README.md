# **Game Club API & MVC Application**
## Demo Video
Watch the demo video on YouTube : https://youtu.be/49y_oukYuPs

![Application Flow Diagram]([images/diagram.png](https://github.com/nguyentien7573/GameClub/blob/main/Diagram.png))

## **Overview**
This project is a **Game Club API and MVC application** designed to manage game clubs and events efficiently. The application allows users to:
- Create clubs
- Schedule events
- Search for existing clubs

The project includes both API endpoints and an MVC UI for ease of interaction.

## **Key Features**
- 🚀 **Create Game Clubs**: Users can create clubs with unique names and descriptions.
- 🔍 **Search Game Clubs**: Provides a search functionality to find clubs by name.
- 📅 **Create Club Events**: Allows users to schedule events for clubs with details like event title, description, and time.
- 👁️ **View Events**: Displays events for a specific club, available through both API and UI.

## **Technologies Used**
- **ASP.NET Core MVC**: To build the web user interface.
- **ASP.NET Core Web API**: For RESTful API endpoints.
- **Custom Caching System**: Ensures fast data retrieval.
- **Queue-based Architecture**: Handles write operations efficiently.
- **Dual Database System**: One database for reads and another for writes, ensuring smooth synchronization.

## **API Documentation**

### **Club Endpoints**
- **POST** `/api/clubs` - Create a new club.
- **GET** `/api/clubs` - Retrieve all clubs.
- **GET** `/api/clubs/search` - Search for clubs using a query parameter.

### **Event Endpoints**
- **POST** `/api/clubs/{clubId}/events` - Create a new event for a specific club.
- **GET** `/api/clubs/{clubId}/events` - Retrieve events for a specific club.
