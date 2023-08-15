# RTTextBox - Rich Text Blazor Component

The RTTextBox Blazor component is a versatile and customizable rich text editor that allows users to easily format and style their text content. With a set of intuitive buttons for various text decorations, such as Bold, Italic, and Underline, users can create visually appealing and well-structured content.

## Features

- **Easy-to-Use Interface:** The RTTextBox component provides a user-friendly interface that allows users to apply different text decorations without any hassle.

- **Customizable Buttons:** Users can choose from a variety of text decoration buttons, including Bold, Italic, Underline, and more, to suit their content styling needs.

- **Rich Formatting Options:** The RTTextBox supports a range of formatting options to enhance the visual appearance of text, helping users create engaging and readable content.

- **Seamless Integration:** Easily integrate the RTTextBox component into your Blazor applications without complex setup or configuration.

## Getting Started

Follow these simple steps to integrate the RTTextBox component into your Blazor project:

1. **Installation:**
    ```shell
    dotnet add package RTTextBox
    ```

2. **Usage:**
    Add the RTTextBox component to your Blazor component's markup, and configure the desired text decoration buttons.
    ```html
    <RTTextBox>
        <RTTextDecorationButton Type="TextDecorationType.Bold" />
        <RTTextDecorationButton Type="TextDecorationType.Italic" />
        <RTTextDecorationButton Type="TextDecorationType.Underline" />
        <!-- Add more buttons as needed -->
    </RTTextBox>
    ```

3. **Accessing Content:**
    Use the `Content` property to get and set the rich text content of the RTTextBox component.
    ```csharp
    string EditorContent { get; set; }

    <RTTextBox Content="@EditorContent">
        <!-- Button configurations -->
    </RTTextBox>
    ```

4. **Styling:**
    Customize the appearance of the RTTextBox and its buttons using CSS to align with your application's design.

## Customization

The RTTextBox component offers various customization options to tailor it to your application's requirements:

- **Button Selection:** Choose which text decoration buttons to display based on your users' needs.

- **Toolbar Placement:** Position the toolbar containing the text decoration buttons at the top, bottom, or any other suitable location.

- **Button Styling:** Adjust the styling of the buttons to match your application's design language.

## Example

Here's a basic example of how you can use the RTTextBox component in your Blazor application:

```html
<RTTextBox>
    <RTTextDecorationButton Type="TextDecorationType.Bold" />
    <RTTextDecorationButton Type="TextDecorationType.Italic" />
    <RTTextDecorationButton Type="TextDecorationType.Underline" />
</RTTextBox>
```

## Contribution

Contributions to the RTTextBox project are welcome! Feel free to open issues, submit pull requests, or provide feedback to help improve the component.

## License

The RTTextBox component is released under the [MIT License](LICENSE).

---

Add a touch of elegance and readability to your Blazor applications with the RTTextBox component. Empower your users to create beautifully formatted content effortlessly. If you have any questions or need assistance, don't hesitate to reach out.

Happy coding!
