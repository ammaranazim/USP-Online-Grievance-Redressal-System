
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title> USP Online Grievance Redressal System</title>

    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/Default.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">

        <!-- Header -->
        <header class="site-header">
            <div class="container header-inner">

                <a href="Default.aspx" class="brand">
                    <img src="Images/usp-logo.png"
                         alt="USP University Logo"
                         class="brand-logo" />

                    <span class="brand-text">
                        <strong>University of Southern Punjab</strong>
                        <small>Multan, Pakistan</small>
                    </span>
                </a>

                <nav class="main-nav">
                    <a class="active" href="#home">Home</a>
                    <a href="#about">About</a>
                    <a href="#features">Features</a>

                    <a href="SelectRole.aspx" class="nav-cta">
                        Get Started <span>→</span>
                    </a>
                </nav>

            </div>
        </header>

        <main>

            <!-- Hero Section -->
            <section class="hero-section" id="home">

                <div class="hero-overlay"></div>

                <div class="container hero-content">
                    <div class="hero-copy">

                        <span class="eyebrow">
                            UNIVERSITY OF SOUTHERN PUNJAB
                        </span>

                        <h1>
                            USP Online
                            <span>Grievance Redressal System</span>
                        </h1>

                        <p>
                            A centralized and transparent platform to submit,
                            manage, track and resolve university Grievances
                            efficiently. Stay informed throughout the process
                            with clear updates and accountable handling.
                        </p>

                        <div class="hero-actions">
                            <a href="SelectRole.aspx" class="btn-primary-custom">
                                Get Started <span>→</span>
                            </a>

                            <a href="#about" class="btn-outline-custom">
                                Learn More
                            </a>
                        </div>

                    </div>
                </div>

                <!-- Compact Features Strip -->
                <div class="hero-bottom">
                    <div class="container feature-strip" id="features">

                        <div class="feature-item">
                            <div class="feature-icon" aria-hidden="true">✓</div>

                            <div>
                                <h3>Easy Submission</h3>
                                <p>
                                    Submit Grievances through a convenient
                                    online process.
                                </p>
                            </div>
                        </div>

                        <div class="feature-item">
                            <div class="feature-icon" aria-hidden="true">↗</div>

                            <div>
                                <h3>Track Progress</h3>
                                <p>
                                    Follow your Grievance status and
                                    receive updates.
                                </p>
                            </div>
                        </div>

                        <div class="feature-item">
                            <div class="feature-icon" aria-hidden="true">◎</div>

                            <div>
                                <h3>Clear Workflow</h3>
                                <p>
                                    Support structured handling and
                                    resolution of Grievances.
                                </p>
                            </div>
                        </div>

                    </div>
                </div>

            </section>

            <!-- Combined About and How It Works Section -->
            <section class="about-section" id="about">
                <div class="container about-content">

                    <span class="section-eyebrow">
                        A BETTER UNIVERSITY EXPERIENCE
                    </span>

                    <h2>Grievances Deserve to Be Heard</h2>

                    <p>
                        The USP Online Grievance Redressal System
                        provides a dedicated digital platform for university
                        students and authorized personnel to manage Grievances
                        through an organized, transparent and accessible process.
                    </p>

                    <div class="about-points" id="how-it-works">

                        <div class="about-point">
                            <span class="point-number">01</span>

                            <div>
                                <h3>Submit</h3>
                                <p>
                                    Provide relevant details and supporting
                                    evidence for your Grievance.
                                </p>
                            </div>
                        </div>

                        <div class="about-point">
                            <span class="point-number">02</span>

                            <div>
                                <h3>Review</h3>
                                <p>
                                    The responsible university personnel
                                    review and handle the Grievance.
                                </p>
                            </div>
                        </div>

                        <div class="about-point">
                            <span class="point-number">03</span>

                            <div>
                                <h3>Track and Resolve</h3>
                                <p>
                                    Follow the progress and view the final
                                    resolution through your portal.
                                </p>
                            </div>
                        </div>

                    </div>

                </div>
            </section>

        </main>

        <!-- Footer -->
        <footer class="site-footer">
            <div class="container footer-inner">

                <div>
                    <strong>University of Southern Punjab, Multan</strong>
                    <p> USP Online Grievance Redressal System</p>
                </div>

                <span>
                    © 2026 University of Southern Punjab
                </span>

            </div>
        </footer>

    </form>
</body>
</html>
