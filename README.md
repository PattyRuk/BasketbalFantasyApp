# Basketball Fantasy League 
A lightweight, self-seeding sports-analytics workspace built using the **ASP.NET Core MVC** web framework and an automated **Entity Framework Core Code-First** architecture. This platform runs completely local and offline, allowing users to manage team franchises, draft real-world NBA stars, and run simulated tournament brackets with dynamic player stat tracking.

## 🔐 Administrative & Testing Login Credentials
The application automatically provisions two localized testing accounts upon initial database instantiation. Use these profiles to navigate and test different access matrix tiers within the interface:

### League Administrator (Global Management)
*   **Email / Username:** `admin@fantasyleague.com`
*   **Password:** `SecureAdmin123!`
*   **Access Tier:** Create and delete tournament schedules, manage global league configurations, and run the background matchup simulation loops.

### Standard Team Manager (Franchise Account)
*   **Email / Username:** `player@fantasyleague.com`
*   **Password:** `PlayerPassword123!`
*   **Access Tier:** Register a custom team franchise, browse the unassigned player market, draft active athletes onto a personal squad lineup, and register for open tournaments.


## Functional Verification Test Loop

To prove all architectural relationships and dynamic stats trackers are fully active across your clean slate, execute this quick workflow live:
1. **Verify Market Size:** Load the home dashboard anonymously and confirm the **Available Free Agents** metric card tracks exactly **90** elite 2026 NBA starters.
2. **Draft a Franchise Roster:** Log in as the Team Manager. Create a new custom franchise team, go to the available players grid page, choose an athlete (e.g., *Jayson Tatum*), and click **Sign Athlete**. Confirm that the dashboard pool count instantly drops to **89**.
3. **Schedule a Bracket:** Log out, log in as the League Administrator, click **Tournament Brackets** > **Schedule New Bracket**, and create an open competition ruleset.
4. **Simulate Gameplay:** Log back in as your Team Manager, browse to the brackets tab, select **Register My Team**, and check off your active roster lines. Switch back to your Admin account and click **Simulate Bracket**. 
5. **Review Retrospectives:** The background code engine will automatically simulate head-to-head match outcomes, assign direct `GameId` hooks to individual box scores, determine the tournament MVP, and redirect you to a beautifully styled, gold-accented championship retrospective summary screen with zero errors or database constraint exceptions!