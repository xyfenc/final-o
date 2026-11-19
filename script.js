// Built by xyfenc | github.com/xyfenc
const navButtons = document.querySelectorAll(".nav-button");
const panels = document.querySelectorAll("[data-page-panel]");
const main = document.querySelector(".main-content");
const textBorder = document.querySelector(".text-border");
const ids = [...navButtons].map((b) => b.dataset.page);

function updateTextBorder() {
    const names = "DELCANO=CUMMINS=NOORA=BALMACEDA=";
    const context = document.createElement("canvas").getContext("2d");
    context.font = getComputedStyle(textBorder).font;
    const textWidth = context.measureText(names).width || 30;
    textBorder.textContent = names.repeat(Math.ceil(window.innerWidth / textWidth) + 2);
}

function showPage(id, updateHash = true) {
    const target = document.getElementById(id);
    if (!target) return;
    const index = ids.indexOf(id);
    panels.forEach((p) => { p.hidden = p !== target; });
    navButtons.forEach((b, i) => {
        const active = i === index;
        b.classList.toggle("active", active);
        active ? b.setAttribute("aria-current", "page") : b.removeAttribute("aria-current");
    });
    main.scrollTop = 0;
    if (updateHash && location.hash !== "#" + id) history.pushState(null, "", "#" + id);
}

navButtons.forEach((b) => b.addEventListener("click", () => showPage(b.dataset.page)));
document.querySelectorAll("[data-back]").forEach((b) => b.addEventListener("click", () => showPage("home")));
document.querySelectorAll("[data-page-link]").forEach((b) => b.addEventListener("click", () => showPage(b.dataset.pageLink)));
window.addEventListener("popstate", () => showPage(location.hash.slice(1) || "home", false));
showPage(location.hash.slice(1) || "home", false);
updateTextBorder();
window.addEventListener("resize", updateTextBorder);
