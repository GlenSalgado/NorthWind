(() => {
    const configuredInputs = new WeakSet();
    const configuredPasswordForms = new WeakSet();
    const configuredRegistrationForms = new WeakSet();
    const managedPasswordInputs = new WeakSet();
    const configuredPasswordToggles = new WeakSet();
    const nameCharacters = /^[\p{L}\s]*$/u;
    const onlyNameCharacters = value => [...value].filter(character => /[\p{L}\s]/u.test(character)).join("");

    function isRegistrationPage() {
        return window.location.pathname.replace(/\/+$/, "").toLowerCase().endsWith("/user/register");
    }

    function fieldText(input) {
        const labels = input.labels ? [...input.labels].map(label => label.textContent || "").join(" ") : "";
        const wrapper = input.closest(".mb-3, .form-group, .form-floating, .field");
        return `${input.id} ${input.name} ${input.placeholder} ${input.getAttribute("aria-label") || ""} ${labels} ${wrapper?.textContent || ""}`.toLowerCase();
    }

    function configureNameInput(input) {
        input.maxLength = 50;
        input.required = true;
        input.setAttribute("autocomplete", "name");
        const validateNonBlank = () => input.setCustomValidity(
            input.value.trim() ? "" : "Este campo no puede contener solo espacios.");
        input.addEventListener("beforeinput", event => {
            if (event.data && !nameCharacters.test(event.data)) event.preventDefault();
        }, true);
        input.addEventListener("input", () => {
            const filtered = onlyNameCharacters(input.value).slice(0, 50);
            if (input.value !== filtered) {
                const cursor = input.selectionStart;
                input.value = filtered;
                if (cursor !== null) input.setSelectionRange(Math.min(cursor, filtered.length), Math.min(cursor, filtered.length));
                input.dispatchEvent(new Event("change", { bubbles: true }));
            }
            validateNonBlank();
        }, true);
        input.addEventListener("blur", validateNonBlank, true);
    }

    function addPasswordVisibilityToggle(input) {
        if (configuredPasswordToggles.has(input)) return;
        configuredPasswordToggles.add(input);
        managedPasswordInputs.add(input);

        const button = document.createElement("button");
        button.type = "button";
        button.className = "btn btn-link btn-sm p-0 mt-1";
        button.textContent = "Mostrar contraseña";
        button.setAttribute("aria-label", "Mostrar contraseña");
        button.setAttribute("aria-pressed", "false");
        button.addEventListener("click", () => {
            const showPassword = input.type === "password";
            input.type = showPassword ? "text" : "password";
            button.textContent = showPassword ? "Ocultar contraseña" : "Mostrar contraseña";
            button.setAttribute("aria-label", button.textContent);
            button.setAttribute("aria-pressed", String(showPassword));
            input.focus();
        });
        input.insertAdjacentElement("afterend", button);
    }

    function configureEmailInput(input) {
        input.type = "email";
        input.maxLength = 254;
        input.required = true;
        input.autocomplete = "email";
    }

    function configurePasswordInputs(inputs, form) {
        for (const input of inputs) {
            if (configuredInputs.has(input)) continue;
            configuredInputs.add(input);
            input.maxLength = 100;
            input.required = true;
            input.autocomplete = "new-password";
            addPasswordVisibilityToggle(input);
        }

        if (inputs.length > 1 && !configuredPasswordForms.has(form)) {
            configuredPasswordForms.add(form);
            const password = inputs[0];
            const confirmation = inputs[1];
            const validateMatch = () => confirmation.setCustomValidity(
                confirmation.value && password.value !== confirmation.value
                    ? "Las contraseñas no coinciden."
                    : "");
            password.addEventListener("input", validateMatch, true);
            confirmation.addEventListener("input", validateMatch, true);
        }
    }

    function configureRegistrationForm(form) {
        if (!isRegistrationPage()) return;

        const inputs = [...form.querySelectorAll("input")];
        const passwordInputs = inputs.filter(input => input.type === "password");
        const emailInputs = inputs.filter(input => input.type === "email" || /email|correo/.test(fieldText(input)));
        const plainInputs = inputs.filter(input => ["text", "search", ""].includes(input.type)
            && !emailInputs.includes(input) && !managedPasswordInputs.has(input));

        configurePasswordInputs(passwordInputs, form);

        emailInputs.forEach(input => {
            if (configuredInputs.has(input)) return;
            configuredInputs.add(input);
            configureEmailInput(input);
        });

        let nameInputs = plainInputs.filter(input => /first.?name|given.?name|nombre|last.?name|surname|apellido/.test(fieldText(input)));
        if (nameInputs.length === 0) nameInputs = plainInputs.slice(0, 2);
        nameInputs.forEach(input => {
            if (configuredInputs.has(input)) return;
            configuredInputs.add(input);
            configureNameInput(input);
        });

        if (!configuredRegistrationForms.has(form)) {
            configuredRegistrationForms.add(form);
            form.addEventListener("submit", event => {
                nameInputs.forEach(input => input.setCustomValidity(
                    input.value.trim() ? "" : "Este campo no puede contener solo espacios."));
                if (!form.checkValidity()) {
                    event.preventDefault();
                    form.reportValidity();
                }
            }, true);
        }
    }

    function applyRegistrationRules() {
        if (!isRegistrationPage()) return;
        document.querySelectorAll("form").forEach(configureRegistrationForm);
    }

    const observer = new MutationObserver(applyRegistrationRules);
    observer.observe(document.documentElement, { childList: true, subtree: true });
    window.addEventListener("popstate", applyRegistrationRules);
    applyRegistrationRules();
})();
