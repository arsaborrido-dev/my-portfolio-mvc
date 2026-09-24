/* =========================================================
   SITE JAVASCRIPT
   ========================================================= */


/* =========================================================
   1. API CONFIGURATION
   =========================================================

   IMPORTANT:

   These URLs are accessed by the user's browser.

   If your MVC application and API are running in Docker,
   do NOT automatically use the Docker service name here.

   For example, this may NOT work in the browser:

       http://api:8080/api/projects

   because "api" is usually only resolvable inside the
   Docker network.

   During development, you might use:

       http://localhost:5001/api/projects

   depending on how your Docker ports are mapped.

   ========================================================= */

const API = {

    projects: "",
    projectDetail: "",
    skills: ""

};


/* =========================================================
   2. PLACEHOLDER SKILLS
   ========================================================= */

const placeholderSkills = [

    "C# Programming",
    "SSIS",
    "SSRS",
    "APIs",
    "ASP.NET"

];


/* =========================================================
   3. PLACEHOLDER PROJECTS
   ========================================================= */

const placeholderProjects = [

    {
        id: 1,

        title: "Business API Platform",

        description:
            "A backend API designed to provide reliable access to business data and application services.",

        skills: [
            "C#",
            "ASP.NET",
            "API"
        ]
    },

    {
        id: 2,

        title: "Data Integration Solution",

        description:
            "An ETL solution for moving, transforming, and validating data between different systems.",

        skills: [
            "SSIS",
            "SQL Server",
            "ETL"
        ]
    },

    {
        id: 3,

        title: "Reporting Dashboard",

        description:
            "A reporting solution that turns operational data into useful business information.",

        skills: [
            "SSRS",
            "SQL Server",
            "Reporting"
        ]
    }

];


/* =========================================================
   4. GENERIC API REQUEST
   ========================================================= */

async function fetchFromApi(url) {

    if (!url) {
        return null;
    }

    try {

        const response = await fetch(url);

        if (!response.ok) {

            throw new Error(
                `API request failed: ${response.status}`
            );

        }

        return await response.json();

    } catch (error) {

        console.error("API Error:", error);

        return null;
    }
}


/* =========================================================
   5. GET ALL PROJECTS

   Corresponds to:

       GetAllProjects()
   ========================================================= */

async function getAllProjects() {

    const apiData =
        await fetchFromApi(API.projects);

    if (apiData) {
        return apiData;
    }

    return placeholderProjects;
}


/* =========================================================
   6. GET PROJECT DETAIL

   Corresponds to:

       GetProjectDetail(id)
   ========================================================= */

async function getProjectDetail(id) {

    if (!API.projectDetail) {

        return placeholderProjects.find(
            project => project.id === id
        );

    }

    const url =
        `${API.projectDetail}/${id}`;

    return await fetchFromApi(url);
}


/* =========================================================
   7. GET SKILLS

   Corresponds to:

       GetSkills()
   ========================================================= */

async function getSkills() {

    const apiData =
        await fetchFromApi(API.skills);

    if (apiData) {
        return apiData;
    }

    return placeholderSkills;
}


/* =========================================================
   8. RENDER SKILLS
   ========================================================= */

async function renderSkills() {

    const container =
        document.getElementById(
            "skills-container"
        );

    if (!container) {
        return;
    }

    const skills =
        await getSkills();

    container.innerHTML = "";


    if (!skills || skills.length === 0) {

        container.innerHTML = `
            <div class="loading">
                No skills available yet.
            </div>
        `;

        return;
    }


    skills.forEach(skill => {

        /*
            Supports:

            "C#"

            and:

            {
                id: 1,
                name: "C#"
            }
        */

        const skillName =
            typeof skill === "string"
                ? skill
                : skill.name;


        const card =
            document.createElement("div");

        card.className =
            "skill-card";

        card.textContent =
            skillName;

        container.appendChild(card);

    });
}


/* =========================================================
   9. RENDER PROJECTS
   ========================================================= */

async function renderProjects() {

    const container =
        document.getElementById(
            "projects-container"
        );

    if (!container) {
        return;
    }

    const projects =
        await getAllProjects();

    container.innerHTML = "";


    if (!projects || projects.length === 0) {

        container.innerHTML = `
            <div class="loading">
                No projects available yet.
            </div>
        `;

        return;
    }


    projects.forEach((project, index) => {

        const card =
            document.createElement("article");

        card.className =
            "project-card";


        const tags =
            project.skills
                ? project.skills
                    .map(skill => `
                        <span class="tag">
                            ${escapeHtml(skill)}
                        </span>
                    `)
                    .join("")
                : "";


        card.innerHTML = `

            <span class="project-number">
                ${String(index + 1).padStart(2, "0")}
            </span>

            <h3>
                ${escapeHtml(project.title)}
            </h3>

            <p>
                ${escapeHtml(project.description)}
            </p>

            <div class="project-tags">
                ${tags}
            </div>

        `;


        container.appendChild(card);

    });
}


/* =========================================================
   10. HTML ESCAPING
   ========================================================= */

function escapeHtml(value) {

    if (
        value === null ||
        value === undefined
    ) {
        return "";
    }

    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}


/* =========================================================
   11. CONTACT FORM
   =========================================================

   Currently front-end only.

   Later this can become:

       POST /api/contact

   ========================================================= */

// const contactForm =
//     document.getElementById("contact-form");

// if (contactForm) {

//     contactForm.addEventListener(
//         "submit",
//         function (event) {

//             event.preventDefault();

//             alert(
//                 "Thank you! The contact form is ready to be connected to your API."
//             );

//         }
//     );

// }


/* =========================================================
   12. CURRENT YEAR
   ========================================================= */

const currentYear =
    document.getElementById("current-year");

if (currentYear) {

    currentYear.textContent =
        new Date().getFullYear();

}


/* =========================================================
   13. INITIALIZE
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        renderSkills();
        renderProjects();

    }
);
