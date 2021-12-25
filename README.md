Activating the message tab:
* why the messages being loaded on the details page?
    - it's because the messages component is a child component of the details page
    - and when the parent component is loaded (details in out case), the child component is loaded as well.
* how can we control that? 
    * not load the messages on details construction?
    * do load when the messages component is clicked (the data is needed)

lets see how to do that:
go to member-detail.component.html.

ok lets test in the browser, we expect the messages to load only on first time needed.

* up next: get directed to the messages.
    * from the button in the member page (next to 'like' button)
    * from from the ✉️ icon in the member card


