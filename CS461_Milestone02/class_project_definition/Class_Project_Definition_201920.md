2019-20 Class Project Inception
=====================================

## Summary of Our Approach to Software Development

[What processes are we following?]  
[What are we choosing to do and at what level of detail or extent?]

<br>

---
---

## Initial Vision Discussion with Stakeholders

**Primary Stakeholder** -- Katimichael Phelpedecky, swimming legend and hopeful entrepreneur

Katimichael's experience being on the US Olympic team led to an appreciation of how advanced tools can help athletes perform at their best.  The problem is those tools are very expensive and require personnel with advanced training, i.e. elite analysts for elite athletes.  They want to create a business to give regular swimming coaches, from high school, club, college, and masters, advanced analytical and predictive tools to help the athletes on their teams.  Katimichael has assembled a team of investors to fund this project and is hiring your team to create the product.

**The product is centered around three core features:**

1. Record, store and provide tracking, viewing and simple stats for race results for swimming athletes.  This would have a number of features found in [Athletic.net](https://www.athletic.net/), which is used for Track and Cross Country running.

<br>

2. Provide complex analysis of athlete performance over time and over different race types, to give coaches deep insight into their athlete's fitness and performance that they cannot get from their own analyses.  This includes machine learning to predict future performance based on records of past race performance, given different training scenarios.  Validation of this feature will enable the next feature.

<br>

3. Create a tool that will optimize a coach's strategy for winning a specific meet.  This feature will automatically assign athletes to specific races based on their predicted race times in order to beat an opponent coach's strategy.  There will be two modes: one in which we have no knowledge of the opponent team's performance, and one where we do have their performance and can predict their times.

<br><br>

---

### Refined Vision Statement -- Team Nerdvana

[Formal-Yet-Preliminary Vision Statement Document
<br> “Team Nerdvana Vision Statement: Elite Aquatics System\Milestone02”](https://stormy9.github.io/nerdvana/TeamNerdvana_VisionStatement_1-0.pdf)

![alt text](https://stormy9.github.io/nerdvana/Vision_Statement_Cap.PNG "Vision Statement")


<br><br>

---
---

## Initial Requirements Elaboration and Elicitation

### Questions

1.  Are we keeping track of meets (as in, by name) and locations?
	* yes!

2.  Should coaches have “admin” status, for creating/updating/deleting their own athlete’s records?
	* In order for the app to be *fully* interactive for coaches, yes -- they should be able to create/update/delete (or mark inactive) their own athlete records.  Newly created/updated records should be available for analysis in real time.

3.  Do you want to provide the images we use on your app, or should we just find some stock photos?  (for other than athlete profile photos, of course)
	* This will be ironed out later

4.  Non-functional Requirement #4 states:  “English will be the default language”.  Do you want to include other languages as well?
	* Eventually, but not until the app is up and smoothly running.

5.  Do you want users to be able to search\filter by factors other than athlete?  For instance, event type?
	* Sure, why not?  This could be a useful feature also.

6.  Should athletes be able to create and manage their own personal profile pages?
	* Yes, but only their profile aka personal details, they will not have access to their race results records.

7.  Should coaches be able to create and manage their own personal profile pages?
	* Yes, for themselves and their team/school.

8.  Should coaches be able to create and manage profile pages for their school and/or team (like as a unit/group)?
	* See above.

9.  Do you want a way for coaches to communicate with their athletes via the app?
	* This could be a useful feature, but let's not worry about it right away.

10. Would you like a way to aggregate/access team/school statistics, so that visitors and coaches can view those as well as individual athletes?
	* Again, this would be a useful and interesting feature, but for now let's just deal with individual athletes.


<br><br>

---

### Interviews

*(we “interviewed” ourselves, acting as both roles; and discussed among ourselves)*

<br><br>

---

### Other Elicitation Activities?


<br><br>

---
---

## List of Primary Needs and Features

1. They want a nice looking site, with a clean light modern style, images that evoke swimming and competition.  (More like [Strava](https://www.strava.com/features) and less like [Athletic.net](https://www.athletic.net/TrackAndField/Division/Event.aspx?DivID=100004&Event=14))  It should be easy to find the features available for free and then have an obvious link to register for an account or log in.  It should be fast and easily navigable.  

<br>

2. The general public will be able to view all results (just the race distance, type and time).  These are public events and the results should be freely available.  They should be able to search by athlete name, team, coach or possibly event date and location.  Not sure if they want to be able to filter or drill down as Athletic.net does.  They're not trying to organize by state, school, etc. Athletes are athletes and it doesn't matter where they're competing.  This is completely general, but only for swimming.

<br>

3. Logins will be required for viewing statistics and all other advanced features.  We eventually plan to offer paid plans for accessing these advanced features.  They'll be free initially and we'll transition to paid plans once we get people hooked.

<br>

4. Admin logins are needed for entering new data.  Only employees and contractors will be allowed to enter, edit or delete data.
    * **Coaches will be given admin status if they request it, in order to enter/update their own athlete's records.**
        * **Optionally they can forward their data to an employee for inputting.**

<br>

5. "Standard" logins are fine.  Use email (must be unique) for username and then require an 8+ character password.  Will eventually need to confirm email to try to prevent some forms of misuse.  Admins and contractors must have an offline confirmation by our employees and then the "super" admin adds them manually.

<br>

6. The core entity is the athlete.  They are essentially free agents in the system.  They can be a member of one or more teams at one time, then change at any time.  Later when we want to have teams and do predictive analysis we'll let the coaches assemble their own teams and add/remove athletes from their rosters.

<br>

7. The first stats we want are: 1) display PR's prominently in each race event, 2) show a historical picture/plot of performance, per race type and distance, 3) some measure of how they rank compared to other athletes, both current and historical, 4) something that shows how often they compete in each race event, i.e. which events are they competing in most frequently, and alternately, which events are they "avoiding"

<br>

8.  **The secondary entity is the coach.  They should be able to access each of their athletes, and eventually assemble teams, and also a profile page to represent their team and/or school.  These pages should have links to each of their athlete’s profile pages, as well as links so the coach can easily view all stats for their athletes.  Also to add/edit/delete swimmers from their team.**

<br>

9. **The ternary entity is the team or school.  A coach should be able to set up a profile page for his or her team, in part for recruitment efforts -- and of course for bragging rights if applicable.**

<br><br>

---
---

## Initial Modeling
we did:

*  flow diagram
*  **sequence diagram(s)**
    ![alt text](https://stormy9.github.io/nerdvana/sequence_diagram.jpg "Sequence Diagram")


<br>

---

### Use Case Diagram(s)

![alt text](https://stormy9.github.io/nerdvana/use_case_diagram.jpg "Use Case Diagram(s)")

<br>

---

### Other Modeling


<br><br>

---
---

## Identify Non-Functional Requirements

1. User accounts and data must be stored indefinitely.  They don't want to delete; rather, mark items as "deleted" but don't actually delete them.  They also used the word "inactive" as a synonym for deleted.

<br>

2. Passwords should not expire

<br>

3. Site should never return debug error pages.  Web server should have a custom 404 page that is cute or funny and has a link to the main index page.

<br>

4. All server errors must be logged so we can investigate what is going on in a page accessible only to Admins.

<br>

5. English will be the default language.

<br>

6. Athlete profiles and data should be stored indefinitely for historical purposes, for both individual athletes and team/school records.

<br>

7. **Users should have the option for the site/app to “remember” them and an option stay logged in unless they specifically log out.**

<br>

8. **Implement a way for users to recover lost/forgotten usernames/passwords.**


<br><br>

---
---

## Identify Functional Requirements (User Stories)

E: Epic  
U: User Story  
T: Task  

1. [U] As a visitor to the site, I would like to see a fantastic and modern homepage that introduces me to the site and the features currently available.

   1. [T] Create starter ASP dot NET MVC 5 Web Application with Individual User Accounts and no unit test project

   2. [T] Choose CSS library (Bootstrap 3, 4, or ?) and use it for all pages

   3. [T] Create nice homepage: write initial content, customize navbar, hide links to login/register

   4. [T] Create SQL Server database on Azure and configure web app to use it. Hide credentials.

   5. **[T] Create easy to use navigation system that is consistent across all pages on the site**

<br><br>

2. [U] As a visitor to the site, I would like to be able to register an account so I will be able to access athlete statistics.

   1. [T] Copy SQL schema from an existing ASP.NET Identity database and integrate it into our UP script

   2. [T] Configure web app to use our db with Identity tables in it

   3. [T] Create a `user` table and customize user pages to display additional data

   4. [T] Re-enable login/register links

   5. [T] Manually test register and login; user should easily be able to see that they are logged in

<br><br>

3. [E] As an administrator of the site/app, I want to be able to upload a spreadsheet of results so that new data can be added to our system.

    1. **[U] As a coach, I want to be able to get my race results into the app quickly so that I can make use of them right away.**

    2. **[U] As an employee/admin, I want to be able to help coaches upload their swimmer/meet data quickly so that we can provide good customer service.**

<br><br>

4. [U] As a visitor to the site, I want to be able to search for an athlete and then view their athlete page, so I can find out more information about them.

    1. **[T] Create a profile template for athlete pages, so the format/styling is consistent across the app, and all information is displayed in the same order/location on the athlete's pages**

    2. **[T] Implement a “Search For An Athlete” feature with a prominent link for visitors**

<br><br>

5. [U] As a visitor to the site, I want to be able to view all race results for an athlete, so I can see how they have performed.

    1. **[T] Create a prominent link on each athlete’s profile page that will retrieve and display all race results for that athlete**

    2. **[T] Create a `race_results` table in our main database**

    3. **[T] Create a nice-looking page on which to display race results**

<br><br>

6. [U] As a visitor to the site, I want to be able to view PR's (personal records) for an athlete so I can see their best performances.

    1. **[T] Create a prominent link on each athlete’s profile page that will retrieve and display that athlete’s personal records across race events**

<br><br>

7. **[U] As a visitor to the site, I want to be able to search for Teams/Schools, and then view their profile page, so I can find out more information about them.**

    1. **[T]    (similar to #4)**

<br><br>

8. **[U] As a visitor to the site, I want to be able to view all race results for a team, so I can see how they have performed as a team.**

    1. **[T]     (similar to #5)**

<br><br>

9. **[E] As a user of the app, I would like to compare the race results of multiple athletes over race types, graphically, so I can see how certain swimmers compare.**

    1. **[U]  As a visitor to the app, I would like to compare race results of my favorite swimmers so I can try to win the office pool.**

    2. **[U]  As a swim coach, I would like to compare race results of the swimmers on my team to other teams, so that I can improve our odds of winning meets.**

    3. **[U]  As an athlete, I would like to see how I'm doing compared to my competitors/opponents.**
    
        1. **[T] Implement a graphing API to visually represent race results**

        2. **[T] Devise a way for a coach to compare athlete-to-athlete**

        3. **[T] Devise a way for a coach to compare team-to-team**

<br><br>

10. [U] As a robot, I would like to be prevented from creating an account on your website, so I don't ask millions of my friends to join your website and try to add comments about male enhancement drugs.

    1. **[T] Incorporate something like the ‘captcha’ feature to the app**

    2. **[T] Implement ability for admins to moderate comments**

<br><br>

11. **[U] As an athlete, I would like to be able to customize my profile page with images and personal details.**

    1. **[T] Create an `athlete` table in our main database**

    2. **[T] Create customizable athlete pages that will display their profile details in an attractive layout that is consistent across all athletes**

<br><br>

12. **[U] As a coach, I would like to be able to cusomize my profile page -- and that of my team/school -- with images and pertinant details.**

    1. **[T] Create a `coach` table in our main database**

    2. **[T] Create a `team_school` table in our main database**

    3. **[T] Create customizable team/school pages that will display their profile details in an attractive layout that is consistent across all coaches and teams/schools**

<br><br>

13. **[U] As an athlete, i would like to be able to send my statistics to coaches/schools to strengthen my recruitment efforts to get into a good swim program and further my athletic career.**

    1. **[T] Include a link on coach/team/school pages, which athletes can click and it will send their info to the coach/school/team of their choice**

    2. **[T] Devise a “form letter email” that would be sent to the coach/team/school that the athlete could personalize**

<br><br>

14. **[U] As a coach, I would like to be able to reach out to athletes for recruitment purposes, to show them how successful they could be on our team.**

    1. **[T] Include a link from athlete pages, which coaches can click and it will send team/school info to a swimmer they are thinking of recruiting**

    2. **[T] Devise a “form letter email” that would be sent to the prospective recruit, that the coach could personalize**

<br><br>

15. **[E] As a coach I want to have a live webcam feed of the practice facility, and the ability to specify recording windows, so that I can monitor performance and technique in order to send snippets of the recording to athletes to help them improve their form.  Also so I can coach from my La-Z-Boy if I so choose.**

    1. **[T] Select and implement an API for making and managing/accessing recordings**

    2. **[T] Determine the best way for coaches to isolate video snippets they want to preserve**

    3. **[T] Decide the best way for coaches to yell instructions to their team from their La-Z-Boy**

<br>

<br><br>

---
---

## Initial Architecture Envisioning

![alt text](https://stormy9.github.io/nerdvana/architecture_modeling.jpg "Our Architecture Modeling")

<br><br>

---
---

## Agile Data Modeling
We did an ER Diagram:

![alt text](https://stormy9.github.io/nerdvana/ERD.jpg "Our ER Diagram")

<br><br>

---
---

## Timeline and Release Plan

* Inception Phase (Week 2)
* Sprint #1 (Week 3)
* Sprint #2 (Week 4)
* Drop this project

<br><br>

---
---


