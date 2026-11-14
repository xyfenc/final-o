const navigationButtons = document.querySelectorAll(".nav-button");
const pagePanels = document.querySelectorAll("[data-page-panel]");
const backButtons = document.querySelectorAll("[data-back]");
const pageLinks = document.querySelectorAll("[data-page-link]");
const mainContent = document.querySelector(".main-content");
const textBorder = document.querySelector(".text-border");

function updateTextBorder() {
    const names = "DELCANO=CUMMINS=NOORA=BALMACEDA=";
    const canvas = document.createElement("canvas");
    const context = canvas.getContext("2d");
    context.font = getComputedStyle(textBorder).font;
    const textWidth = context.measureText(names).width || 30;
    const repetitions = Math.ceil(window.innerWidth / textWidth) + 2;
    textBorder.textContent = names.repeat(repetitions);
}

function showPage(pageId) {
    const targetPanel = document.getElementById(pageId);
    if (!targetPanel) {
        return;
    }

    pagePanels.forEach((panel) => {
        panel.hidden = panel !== targetPanel;
    });

    navigationButtons.forEach((button) => {
        const isActive = button.dataset.page === pageId;
        button.classList.toggle("active", isActive);
        if (isActive) {
            button.setAttribute("aria-current", "page");
        } else {
            button.removeAttribute("aria-current");
        }
    });

    mainContent.scrollTop = 0;
}

navigationButtons.forEach((button) => {
    button.addEventListener("click", () => showPage(button.dataset.page));
});

backButtons.forEach((button) => {
    button.addEventListener("click", () => showPage("home"));
});

pageLinks.forEach((button) => {
    button.addEventListener("click", () => showPage(button.dataset.pageLink));
});

updateTextBorder();
window.addEventListener("resize", updateTextBorder);
showPage("home");
