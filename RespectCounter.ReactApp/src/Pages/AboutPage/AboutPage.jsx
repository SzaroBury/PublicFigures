import React from 'react';
import Container from 'react-bootstrap/Container'
import "./AboutPage.css";

class AboutPage extends React.Component {
    static displayName = AboutPage.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    render() {
        return (
            <Container className="about-page">
                <h1 className="text-center">Welcome To Respect Counter</h1>
                <p className="m-5 text-center">.NET + React application that allows users to express their opinion on various public figures, their quotes and actions.</p>

                <div>
                    <h5>Main functionalities:</h5>
                    <ul>
                        <li>Users can propose persons.</li>
                        <li>Users can add quotes/actions and link them to stored persons.</li>
                        <li>Users can comment and react to persons, activities and other comments.</li>
                        <li>Users can search quotes, activities and persons by tags.</li>
                        <li>Admin can verify persons, quotes and activities added by users.</li>
                        <li>Admin can create verified persons, quotes and activities.</li>
                        <li>Admin can hide persons, activities and comments.</li>
                    </ul>
                </div>
                <h5>To-do list: </h5>
                <ul>
                    <li>Avatar images</li>
                    <li>User settings</li>
                    <li>Reporting activities, comments...</li>
                    <li>More data for testing</li>
                </ul>
                <h5>Errors to fix:</h5>
                <ul>
                    <li></li>
                </ul>
            </Container>
        );
    }
}

export default AboutPage;